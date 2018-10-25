using Innocellence.RoyaltyScan.Services;
using System.Web.Http;
using System.Web.Mvc;

namespace Innocellence.RoyaltyScan.Controllers
{
    public class SoftwaresController : Controller
    {
        ISoftwareService _softwareService;
        public SoftwaresController(ISoftwareService softwareService)
        {
            _softwareService = softwareService;
        }

        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        public JsonResult Post([FromBody]SubmitSoftwaresView value)
        {
            var r = _softwareService.Save(value.UserName, value.MachineName, value.Softwares);
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}