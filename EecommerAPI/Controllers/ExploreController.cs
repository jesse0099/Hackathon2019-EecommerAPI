using BL;
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
            proc = new Procedimientos();
            Thread.Sleep(2000);
            return Ok(proc.GetCategories());
        }
    }
}