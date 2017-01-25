using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp1.Resources;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using Windows.Networking.PushNotifications;
using System.IO;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using Microsoft.Phone.Notification;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {       
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private string ip = "http://localhost:64535/";

        // Identificação do canal de notificação
        private string channelName = "channeIfrn";

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Recupera o canal da notificação se existir
            HttpNotificationChannel httpChannel = HttpNotificationChannel.Find(channelName);

            try
            {
                // Verifica se o canal de notificação existe
                if (httpChannel == null)
                {
                    // Canal de notificação não existe, instancia novo canal.
                    httpChannel = new HttpNotificationChannel(channelName);

                    // Delegates para atualização, erro e recebimento de mensagem
                    httpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(httpChannel_ChannelUriUpdated);
                    httpChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(httpChannel_ErrorOccurred);
                    httpChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(httpChannel_ShellToastNotificationReceived);

                    // Abre o canal de notificação com o Microsoft Push Notification Service
                    httpChannel.Open();

                    // Efetiva a ligação com o canal de notificação
                    httpChannel.BindToShellToast();
                }
                else
                {
                    // Canal existe

                    // Delegates para atualização, erro e recebimento de mensagem
                    //httpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(httpChannel_ChannelUriUpdated);
                    httpChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(httpChannel_ErrorOccurred);
                    httpChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(httpChannel_ShellToastNotificationReceived);

                    // Mostra dados do canal
                    System.Diagnostics.Debug.WriteLine(httpChannel.ChannelUri.ToString());
                    // MessageBox.Show(String.Format("O canal URI é {0}", httpChannel.ChannelUri.ToString()));

                    // Registra o usuário no serviço de usuários
                    atualizarUsr(txtNome.Text, httpChannel.ChannelUri.ToString());
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void httpChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                // Mostra dados do canal
                System.Diagnostics.Debug.WriteLine(e.ChannelUri.ToString());
                // MessageBox.Show(String.Format("O canal URI é {0}", e.ChannelUri.ToString()));

                // Registra o usuário no serviço de usuários
                atualizarUsr(txtNome.Text, e.ChannelUri.ToString());
            });
        }

        private void httpChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            // Mostra dados do erro
            Dispatcher.BeginInvoke(() => MessageBox.Show(String.Format("A push notification {0} error occurred.  {1} ({2}) {3}",
                e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData)));
        }

        private void httpChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            // Notificação recebida com o aplicativo aberto
            // Este método é chamado e os dados vem no parâmetro e.Collection (NotificationEventArgs)

            // Página destino
            string relativeUri = string.Empty;

            // Mensagem apresentada ao usuário
            StringBuilder msg = new StringBuilder();
            msg.AppendFormat("Toast Recebido {0}:\n", DateTime.Now.ToShortTimeString());

            // Recupera dados da notificação
            foreach (string key in e.Collection.Keys)
            {
                msg.AppendFormat("{0}: {1}\n", key, e.Collection[key]);

                // Página destino
                if (string.Compare(key, "wp:Param",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.CompareOptions.IgnoreCase) == 0)
                {
                    relativeUri = e.Collection[key];
                }
            }

            // Mostra dados da notificação
            Dispatcher.BeginInvoke(() =>
            {
                // Mensagem apresentada ao usuário
                MessageBox.Show(msg.ToString());
            });
        }

        private async void atualizarUsr(string nome, string uri)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/api/Usuario/");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> obj = JsonConvert.DeserializeObject<List<Models.Usuario>>(str);
            Models.Usuario usr = obj.Find(x => x.Nome == nome);
            if (usr != null)
            {
                Models.Usuario f = new Models.Usuario
                {
                    Nome = usr.Nome,
                    Photo = usr.Photo,
                    Uri = uri,
                };
                string s = "=" + JsonConvert.SerializeObject(f);
                var content = new StringContent(s, Encoding.UTF8, "application/x-www-form-urlencoded");
                await httpClient.PutAsync("/api/Usuario/" + usr.Id, content);
                this.NavigationService.Navigate(new Uri("/Zap.xaml?usuario=" + usr.Nome, UriKind.Relative));
            }
            else MessageBox.Show("Usuário Inválido!");
        }
    }
}