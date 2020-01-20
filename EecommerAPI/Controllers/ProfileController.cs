using BE;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EecommerAPI.Controllers
{
    public class ProfileController : ApiController
    {


        [Authorize]
        [RoutePrefix("api/clients")]
        public class CustomersController : ApiController
        {
            Procedimientos proc;

            [HttpGet]
            [Route("profileInfo")]
            public IHttpActionResult GetInfo(string uniquename)
            {
                proc = new Procedimientos();
                ClientProfile returned = proc.GetClientProfile(uniquename);
                return Ok(returned);
            }

            [HttpGet]
            [Route("GetAllCustomers")]
            public IHttpActionResult GetAll()
            {
                var customersFake = new string[] { "customer-1", "customer-2", "customer-3" };
                return Ok(customersFake);
            }
        }
    }
}
