using BL;
using System;
using System.Threading;
using System.Web.Http;

namespace EecommerAPI.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Explore")]
    public class ExploreController: ApiController
    {
        Procedimientos proc;

        [HttpGet]
        [Route("Categories")]
        public IHttpActionResult GetCategories() {
            try
            {
                proc = new Procedimientos();
                Thread.Sleep(2000);
                return Ok(proc.GetCategories());
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }
    }
}