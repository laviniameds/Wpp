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
using Microsoft.Phone.Notification;
using System.Net.Http;
using Windows.Networking.PushNotifications;
using System.IO;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

namespace PhoneApp1
{
    public partial class Zap : PhoneApplicationPage
    {
        private string ip = "http://localhost:64535/";
        private static Models.Usuario u;
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string n = "";
            if (NavigationContext.QueryString.TryGetValue("usuario", out n))
                ReceberUser(n);
        }

        private async void ReceberUser(string nomeU)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/api/Usuario/");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> obj = JsonConvert.DeserializeObject<List<Models.Usuario>>(str);
            u = new Models.Usuario();
            u = obj.Find(x => x.Nome == nomeU);
            ExibirConversas();
            //ExibiGrupos();
        }

        // Image stream variables
        private MemoryStream photoStream;
        private string fileName;
        // PhotoChooserTask definition
        PhotoChooserTask photoChooserTask;

        public Zap()
        {
            InitializeComponent();
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(OnPhotoChooserTaskCompleted);
        }

        private void OnChoosePicture(object sender, System.Windows.Input.GestureEventArgs e)
        {
            photoChooserTask.Show();
        }

        private void OnPhotoChooserTaskCompleted(object sender, PhotoResult e)
        {
            // Hide text messages
            //txtError.Visibility = Visibility.Collapsed;
            //txtMessage.Visibility = Visibility.Collapsed;
            // Make sure the PhotoChooserTask is resurning OK
            if (e.TaskResult == TaskResult.OK)
            {
                // initialize the result photo stream
                photoStream = new MemoryStream();
                // Save the stream result (copying the resulting stream)
                e.ChosenPhoto.CopyTo(photoStream);
                // save the original file name
                fileName = e.OriginalFileName;
                // display the chosen picture
                var bitmapImage = new BitmapImage();
                bitmapImage.SetSource(photoStream);
                imgSelectedImage.Source = bitmapImage;
                // enable the upload button
                btnInserirContato.IsEnabled = true;
            }
            else
            {
                // if result is not ok, make sure user can't upload
                btnInserirContato.IsEnabled = false;
            }
        }

        private async void UploadFile()
        {
            // Make sure there is a picture selected
            if (photoStream != null)
            {
                // initialize the client
                // need to make sure the server accepts network IP-based
                // requests. 
                // ensure correct IP and correct port address
                var fileUploadUrl = @ip + "fileupload";
                var client = new HttpClient();
                //client.BaseAddress = new Uri(ip);
                // Reset the photoStream position
                // If you don't reset the position, the content lenght
                // sent will be 0
                photoStream.Position = 0;
                // This is the postdata
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StreamContent(photoStream), "file", fileName);
                // upload the file sending the form info and ensure a result.
                // it will throw an exception if the service doesn't return 
                // a valid successful status code
                await client.PostAsync(fileUploadUrl, content).ContinueWith((postTask) =>
                {
                    postTask.Result.EnsureSuccessStatusCode();
                });
            }
        }

        private async void ExibirConversas()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/api/Usuario/");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> obj = JsonConvert.DeserializeObject<List<Models.Usuario>>(str);
            List<Models.Usuario> lf = new List<Models.Usuario>();
            foreach (Models.Usuario k in obj) if (k.Nome != u.Nome) lf.Add(k);
            ListContatos.ItemsSource = lf;
            ListSelectUsers.ItemsSource = lf;
            ListUsersSend.ItemsSource = lf;
        }

        private async void ExibiGrupos()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/api/Grupo/");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Grupo> obj = JsonConvert.DeserializeObject<List<Models.Grupo>>(str);
            ListGrupos.ItemsSource = obj;
            ListSelectGPEdit.ItemsSource = obj;
        }

        private async void btnEnviarMsg_Click(object sender, RoutedEventArgs e)
        {
            // Envia a mensagem diretamente para o Microsoft Push Notification Service
            if (ListUsersSend.SelectedIndex == -1)
            {
                MessageBox.Show("Usuário não selecionado");
                return;
            }
                // Mensagem: toast notification
                string msg =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<wp:Notification xmlns:wp=\"WPNotification\">" +
                    "<wp:Toast>" +
                        "<wp:Text1>" + txtMsg.Text + "</wp:Text1>" +
                        "<wp:Param>/Notification.xaml?Msg1="
                            + txtMsg.Text + "</wp:Param>" +
                    "</wp:Toast>" +
                "</wp:Notification>";

                // Codifica a mensagem a ser enviada
                byte[] msgBytes = Encoding.UTF8.GetBytes(msg);

                // Cria a requisição web com a notificação para a o usuário selecionado
                string uri = (ListUsersSend.SelectedItem as Models.Usuario).Uri;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "Post";
                request.ContentType = "text/xml";
                request.ContentLength = msg.Length;
                request.Headers["X-MessageID"] = Guid.NewGuid().ToString();
                request.Headers["X-WindowsPhone-Target"] = "toast";
                request.Headers["X-NotificationClass"] = "2";

                // Envia a requisição web 
                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    requestStream.Write(msgBytes, 0, msgBytes.Length);
                }

                // Envia a mensagem para o serviço de usuários
                // O serviço de usuários envia para o Microsoft Push Notification Service
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ip);

                Models.Mensagem m = new Models.Mensagem
                {
                    Uri = (ListUsersSend.SelectedItem as Models.Usuario).Uri,
                    Texto1 = txtMsg.Text,
                    Param = "Notification.xaml"
                };
                string s = "=" + JsonConvert.SerializeObject(m);
                var content = new StringContent(s, Encoding.UTF8,
                    "application/x-www-form-urlencoded");
                await httpClient.PostAsync("/api/mensagem", content);

            MessageBox.Show("Mensagem enviada!");
        }

        private async void btnInserirContato_Click(object sender, RoutedEventArgs e)
        {
            UploadFile();
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/api/Usuario/");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> obj = JsonConvert.DeserializeObject<List<Models.Usuario>>(str);
            Models.Usuario usr = obj.Find(x => x.Nome == txtNome.Text);
            if (usr == null)
            {
                Models.Usuario u = new Models.Usuario
                {
                    Nome = txtNome.Text,
                    Uri = "",
                    Photo = ""
                };
                List<Models.Usuario> ul = new List<Models.Usuario>();
                ul.Add(u);
                string s = "=" + JsonConvert.SerializeObject(ul);
                var content = new StringContent(s, Encoding.UTF8, "application/x-www-form-urlencoded");
                await httpClient.PostAsync("/api/Usuario/", content);
                MessageBox.Show("Inserido com sucesso!");
                ExibirConversas();
                btnInserirContato.IsEnabled = false;
                // reset the image control
                imgSelectedImage.Source = null;
            }
            else MessageBox.Show("Nome de usuário já existe!");
        }

        private async void btnInserirGrupo_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            Models.Grupo g = new Models.Grupo
            {
                Descricao = txtDescricao.Text,
                IdAdm = u.Id
            };
            List<Models.Grupo> gl = new List<Models.Grupo>();
            gl.Add(g);
            string s = "=" + JsonConvert.SerializeObject(gl);
            var content = new StringContent(s, Encoding.UTF8, "application/x-www-form-urlencoded");
            await httpClient.PostAsync("/api/Grupo/", content);
            MessageBox.Show("Inserido com sucesso!");
            ExibiGrupos();
        }

        private async void btnEditContato_Click(object sender, RoutedEventArgs e)
        {
            Models.Usuario obj = (Models.Usuario)ListSelectUsersEdit.SelectedItem;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            Models.Usuario f = new Models.Usuario
            {
                Nome = txtNomeEdit.Text,
                Photo = txtFotoEdit.Text,
                Uri = obj.Uri,
            };
            string s = "=" + JsonConvert.SerializeObject(f);
            var content = new StringContent(s, Encoding.UTF8, "application/x-www-form-urlencoded");
            await httpClient.PutAsync("/api/Usuario/" + obj.Id, content);
            MessageBox.Show("Atualizado com sucesso!");
            ExibirConversas();
        }

        private async void btnDelContato_Click(object sender, RoutedEventArgs e)
        {
            Models.Usuario obj = (Models.Usuario)ListSelectUsersEdit.SelectedItem;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            await httpClient.DeleteAsync("/api/Usuario/" + obj.Id.ToString());
            MessageBox.Show("Deletado com sucesso!");
            ExibirConversas();
        }

        private async void btnEditGP_Click(object sender, RoutedEventArgs e)
        {
            Models.Grupo obj = (Models.Grupo)ListSelectGPEdit.SelectedItem;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            Models.Grupo f = new Models.Grupo
            {
                Descricao = txtNomeEdit.Text,
                IdAdm = obj.IdAdm
            };
            string s = "=" + JsonConvert.SerializeObject(f);
            var content = new StringContent(s, Encoding.UTF8, "application/x-www-form-urlencoded");
            await httpClient.PutAsync("/api/Grupo/" + f.Id, content);
            MessageBox.Show("Atualizado com sucesso!");
            ExibiGrupos();
        }

        private async void btnDelGP_Click(object sender, RoutedEventArgs e)
        {
            Models.Grupo obj = (Models.Grupo)ListSelectGPEdit.SelectedItem;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            await httpClient.DeleteAsync("/api/Usuario/" + obj.Id.ToString());
            MessageBox.Show("Deletado com sucesso!");
            ExibiGrupos();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Notification.xaml", UriKind.Relative));
        }
    }
}