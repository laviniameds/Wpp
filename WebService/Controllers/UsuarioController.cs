using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebService.Controllers
{
    public class UsuarioController : ApiController
    {
        // GET api/usuario
        public IEnumerable<Models.Usuario> Get()
        {
            Models.ZapDataContext dc = new Models.ZapDataContext();
            var r = from u in dc.Usuarios select u;
            return r.ToList();
        }

        // POST api/usuario
        public void Post([FromBody] string value)
        {
            List<Models.Usuario> x = JsonConvert.DeserializeObject<List<Models.Usuario>>(value);
            Models.ZapDataContext dc = new Models.ZapDataContext();
            dc.Usuarios.InsertAllOnSubmit(x);
            dc.SubmitChanges();
        }

        // PUT api/usuario
        public void Put(int id, [FromBody] string value)
        {
            Models.Usuario x = JsonConvert.DeserializeObject<Models.Usuario>(value);
            Models.ZapDataContext dc = new Models.ZapDataContext();
            Models.Usuario usr = (from u in dc.Usuarios
                                  where u.Id == id
                                  select u).Single();
            usr.Nome = x.Nome;
            usr.Uri = x.Uri;
            usr.Photo = x.Photo;
            dc.SubmitChanges();
        }

        // DELETE api/usuario
        public void Delete(int id)
        {
            Models.ZapDataContext dc = new Models.ZapDataContext();
            Models.Usuario usr = (from u in dc.Usuarios
                                  where u.Id == id
                                  select u).Single();
            dc.Usuarios.DeleteOnSubmit(usr);
            dc.SubmitChanges();
        }
    }
}
