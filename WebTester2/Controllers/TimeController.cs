using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebTester2.Controllers
{
    [Produces("application/json")]
    [Route("api/time")]
    public class TimeController : Controller
    {
        [HttpGet]
        public string Get( )
        {
          return DateTime.UtcNow.ToLongDateString();
        }
  }
}
