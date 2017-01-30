using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebService.Controllers
{
    public class GrupoUsuarioController : ApiController
    {
        // GET api/grupoUsuario
        public IEnumerable<Models.GrupoUsuario> Get()
        {
            Models.ZapDataContext dc = new Models.ZapDataContext();
            var r = from u in dc.GrupoUsuarios
                    select u;
            return r.ToList();
        }

        // POST api/grupoUsuario
        public void Post([FromBody] string value)
        {
            List<Models.GrupoUsuario> x = JsonConvert.DeserializeObject<List<Models.GrupoUsuario>>(value);
            Models.ZapDataContext dc = new Models.ZapDataContext();
            dc.GrupoUsuarios.InsertAllOnSubmit(x);
            dc.SubmitChanges();
        }

        // PUT api/grupoUsuario
        public void Put(int idG, int IdU, [FromBody] string value)
        {
            Models.GrupoUsuario x = JsonConvert.DeserializeObject<Models.GrupoUsuario>(value);
            Models.ZapDataContext dc = new Models.ZapDataContext();
            Models.GrupoUsuario gprUsr = (from u in dc.GrupoUsuarios
                                          where u.IdGrupo == idG && u.IdUsuario == IdU
                                          select u).Single();
            gprUsr.IdGrupo = x.IdGrupo;
            gprUsr.IdUsuario = x.IdUsuario;
            dc.SubmitChanges();
        }

        // DELETE api/grupoUsuario
        public void Delete(int idG, int IdU)
        {
            Models.ZapDataContext dc = new Models.ZapDataContext();
            Models.GrupoUsuario gprUsr = (from u in dc.GrupoUsuarios
                                          where u.IdGrupo == idG && u.IdUsuario == IdU
                                          select u).Single();
            dc.GrupoUsuarios.DeleteOnSubmit(gprUsr);
            dc.SubmitChanges();
        }
    }
}
