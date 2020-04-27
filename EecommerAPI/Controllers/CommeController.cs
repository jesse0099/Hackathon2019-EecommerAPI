﻿
namespace EecommerAPI.Controllers
{
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


    }
}