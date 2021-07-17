
namespace EecommerAPI.Controllers
{
    using BE;
    using BL;
    using System;
    using System.Collections.Generic;
    using System.Net;
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
            List <Enterprise> returned = proc.getAllComme();
            return Ok(returned);
        }

        [HttpGet]
        [Route("GetAllOrm")]
        public IHttpActionResult GetAllCommer() {
            try
            {
                proc = new Procedimientos();
                return Ok(proc.getAllCommerOrm());
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }

        }

        [HttpGet]
        [Route("getByCat")]
        public IHttpActionResult getByCat(string category) {
            try
            {
                proc = new Procedimientos();
                List<Enterprise> returned = proc.getByCat(category);
                return Ok(returned);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
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
