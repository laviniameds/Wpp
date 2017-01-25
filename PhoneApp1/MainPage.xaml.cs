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

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/api/Usuario/");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> obj = JsonConvert.DeserializeObject<List<Models.Usuario>>(str);
            Models.Usuario usr = obj.Find(x => x.Nome == txtNome.Text);
            if (usr != null)
            {
                this.NavigationService.Navigate(new Uri("/Zap.xaml?usuario=" + usr.Nome, UriKind.Relative));
            }
            else MessageBox.Show("Usuário Inválido!");
        }
    }
}