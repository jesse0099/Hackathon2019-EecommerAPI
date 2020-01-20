using BL;
using EecommerAPI.Models;
using System;
using System.Net;
using System.Threading;
using System.Web.Http;


namespace EecommerAPI.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        Procedimientos proc;

        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }


        [HttpPost]
        [Route("authenticateclient")]
        public IHttpActionResult Authenticate(LoginClientRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            proc = new Procedimientos();
            bool isCredentialValid = proc.LoginClient(login.User, login.Password);
            if (isCredentialValid)
            {
                var token = TokenGenerator.GenerateTokenJwt(login.User);
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }


    }
}
