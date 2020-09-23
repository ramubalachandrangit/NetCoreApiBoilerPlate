using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;
        protected readonly IService _iService;

        public BaseController(IService service, ILogger<BaseController> logger)
        {
            _iService = service;
            _logger = logger;
        }
    }
}
