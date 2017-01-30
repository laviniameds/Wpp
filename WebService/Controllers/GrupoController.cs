using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebService.Controllers
{
    public class GrupoController : ApiController
    {
        // GET api/grupo
        public IEnumerable<Models.Grupo> Get()
        {
            Models.ZapDataContext dc = new Models.ZapDataContext();
            var r = from g in dc.Grupos
                    select g;
            return r.ToList();
        }

        // POST api/grupo
        public void Post([FromBody] string value)
        {
            List<Models.Grupo> x = JsonConvert.DeserializeObject<List<Models.Grupo>>(value);
            Models.ZapDataContext dc = new Models.ZapDataContext();
            dc.Grupos.InsertAllOnSubmit(x);
            dc.SubmitChanges();
        }

        // PUT api/grupo
        public void Put(int id, [FromBody] string value)
        {
            Models.Grupo x = JsonConvert.DeserializeObject<Models.Grupo>(value);
            Models.ZapDataContext dc = new Models.ZapDataContext();
            Models.Grupo gpr = (from u in dc.Grupos
                                where u.Id == id
                                select u).Single();
            gpr.Descricao = x.Descricao;
            gpr.IdAdm = x.IdAdm;
            dc.SubmitChanges();
        }

        // DELETE api/grupo
        public void Delete(int id)
        {
            Models.ZapDataContext dc = new Models.ZapDataContext();
            Models.Grupo gpr = (from u in dc.Grupos
                                where u.Id == id
                                select u).Single();
            dc.Grupos.DeleteOnSubmit(gpr);
            dc.SubmitChanges();
        }
    }
}
