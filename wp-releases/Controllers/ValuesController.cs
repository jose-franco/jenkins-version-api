using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace wp_releases.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHostingEnvironment _host;
        const string DefaultVersion = "1.0.0";
        public ValuesController(IHostingEnvironment host)
        {
            _host = host;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Consulte al desarrollador para entender como funciona esta api." };
        }

        // GET api/values/5
        [HttpGet("getlast/{id}")]
        public ActionResult<string> Get(string id)
        {
            bool exist = false;
            return GetCurrentVersion(id, out exist);
        }


        [HttpGet("newrelease/{id}")]
        public ActionResult<string> GetRelease(string id)
        {
            bool exist = false;
            string version = GetCurrentVersion(id, out exist);
            if (exist)
            {
                string versionparent = (int.Parse(version.Split(".")[0])).ToString();
                string versionnumber = (int.Parse(version.Split(".")[1]) + 1).ToString();
                string versionstring = string.Format("{0}.{1}.0", versionparent, versionnumber);
                SaveCurrentVersion(id, versionstring);
                return versionstring;
            }
            return version;
        }


        [HttpGet("newsnapshot/{id}")]
        public ActionResult<string> GetSnapshot(string id)
        {
            bool exist = false;
            string version = GetCurrentVersion(id, out exist);
            if (exist)
            {
                string versionparent = (int.Parse(version.Split(".")[0])).ToString();
                string versionnumber = (int.Parse(version.Split(".")[1])).ToString();
                string versionnumber2 = (int.Parse(version.Split(".")[2]) + 1).ToString();
                string versionstring = string.Format("{0}.{1}.{2}", versionparent, versionnumber, versionnumber2);
                SaveCurrentVersion(id, versionstring);
            }
            return version;
        }


        private string GetCurrentVersion(string id, out bool exist)
        {
            if (string.IsNullOrEmpty(id)) throw new Exception("Debe especificar la app");

            string filename = System.IO.Path.Combine(_host.ContentRootPath, "App_Data", string.Format("{0}.txt", id.ToUpper().Trim()));

            if (!System.IO.File.Exists(filename))
            {

                System.IO.File.WriteAllText(filename, DefaultVersion, Encoding.UTF8);
                exist = false;
                return DefaultVersion;
            }
            exist = true;
            return System.IO.File.ReadAllText(filename);
        }

        private void SaveCurrentVersion(string id, string versionstring)
        {
            if (string.IsNullOrEmpty(id)) throw new Exception("Debe especificar la app");
            string filename = System.IO.Path.Combine(_host.ContentRootPath, "App_Data", string.Format("{0}.txt", id.ToUpper().Trim()));


            System.IO.File.WriteAllText(filename, versionstring, Encoding.UTF8);
        }



 
    }
}
