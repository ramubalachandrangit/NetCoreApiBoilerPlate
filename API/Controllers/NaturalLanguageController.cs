using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NaturalLanguageController : BaseController
    {
        public NaturalLanguageController(IService service, ILogger<NaturalLanguageController> logger) : base(service, logger)
        {

          
        }

        [HttpPost("Analyse")]
        [AllowAnonymous]
        public async Task<ActionResult> Analyse([FromBody] string sentence, CancellationToken ct = new CancellationToken())
        {
            return Ok();
        }
    }
}
