
namespace EecommerAPI.Controllers
{
    using BE;
    using BL;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    [AllowAnonymous]
    [RoutePrefix("api/comme")]
    public class CommeController : ApiController
    {
        Procedimientos proc ;

        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult getAllComme() {
            proc  = new Procedimientos();
            List <Comercio> returned = proc.getAllComme();
            return Ok(returned);
        }

        [HttpGet]
        [Route("getByCat")]
        public IHttpActionResult getByCat(string category) {
            proc = new Procedimientos();
            List<Comercio> returned = proc.getByCat(category);
            return Ok(returned);
        }

        [HttpGet]
        [Route("GetByCats")]
        public IHttpActionResult GetByCats([FromUri]List<int> vals) {
            if (vals == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            try
            {
                proc = new Procedimientos();
                return Ok(proc.GetByCats(vals));
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("getSucByCommer")]
        public IHttpActionResult getSucByCommer(int idCommer) {
            proc = new Procedimientos();
            List<Sucursal> returned = proc.getSucByCommer(idCommer);
            return Ok(returned);
        }

    }
}
