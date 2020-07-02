using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EecommerAPI.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {
        Procedimientos proc;

        [HttpGet]
        [Route("getByCommer")]
        public IHttpActionResult getByCommer(int idCommer) {
            proc = new Procedimientos();
            return Ok(proc.getByCommer(idCommer));
        }
    }
}
