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
            [Route("profileInfoByAuth")]
            public IHttpActionResult GetProfileInfo(string user, string password) {
                proc = new Procedimientos();
                ClientProfile returned = proc.GetClientProfile(password, user);
                return Ok(returned);
            }

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

            [HttpPut]
            [Route("UpdateClientProfile")]
            public IHttpActionResult UpdateClientProfile(ClientProfile newValue) {
                proc = new Procedimientos();
                return Ok(proc.UpdateClientProfile(newValue));
            }

            [HttpPut]
            [Route("UpdateClientCredentials")]
            public IHttpActionResult UpdateClientCredentials(ClientCredentials newValue) {
                proc = new Procedimientos();
                return Ok(proc.UpdateClientCredentials(newValue));
            }

        }
    }
}
