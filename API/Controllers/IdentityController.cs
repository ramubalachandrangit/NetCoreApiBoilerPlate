using API.Configurations;
using Domain.ApiModels;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class IdentityController : BaseController
    {
        private readonly JwtSettings _jwtSettings;
        private readonly AppSettings _appSettings;
        public IdentityController(IService iService, ILogger<IdentityController> logger, JwtSettings jwtSettings, AppSettings appSettings) : base(iService, logger)
        {
            _jwtSettings = jwtSettings;
            _appSettings = appSettings;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginApiModel cred, CancellationToken ct = new CancellationToken())
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException(ValidationMessage.InvalidInput, ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)).ToList());
            }            
            return Ok(await _iService.ValidateUser(cred, _jwtSettings.Secret, _jwtSettings.TokenLifeTime, ct));
        }
    }
}
