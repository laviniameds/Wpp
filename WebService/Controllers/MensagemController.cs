using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace WebService.Controllers
{
    public class MensagemController : ApiController
    {
        // POST api/Mensagem
        public void Post([FromBody] string value)
        {
            Models.Mensagem m = JsonConvert.DeserializeObject<Models.Mensagem>(value);

            // Mensagem: toast notification
            string msg =
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<wp:Notification xmlns:wp=\"WPNotification\">" +
                "<wp:Toast>" +
                    "<wp:Text1>" + m.Texto1 + "</wp:Text1>" +
                    "<wp:Text2>" + m.Texto2 + "</wp:Text2>" +
                    "<wp:Param>/" + m.Param + "?Msg1="
                            + m.Texto1 + "&amp;Msg2=" + m.Texto2 + "</wp:Param>" +
                "</wp:Toast>" +
            "</wp:Notification>";

            // byte[] msgBytes = new UTF8Encoding().GetBytes(msg);
            byte[] msgBytes = Encoding.Default.GetBytes(msg);

            // create a web request that identifies the payload as a toast notification
            // Cria a requisição para a Uri selecionada
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m.Uri);
            request.Method = "Post";
            request.ContentType = "text/xml";
            request.ContentLength = msg.Length;
            request.Headers["X-MessageID"] = Guid.NewGuid().ToString();
            request.Headers["X-WindowsPhone-Target"] = "toast";
            request.Headers["X-NotificationClass"] = "2";

            // Envia a requisição
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(msgBytes, 0, msgBytes.Length);
            }
        }
    }
}
