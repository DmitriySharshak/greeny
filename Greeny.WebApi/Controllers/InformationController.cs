using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Greeny.WebApi.Models;

namespace Greeny.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiVersion("1.0")]
    public class InformationController
    {
        private readonly IConfiguration _settings;

        public InformationController(IConfiguration settings)
        {
            _settings = settings;
        }

        [HttpGet("/version")]
        public VersionModel GetApiVersion()
        {
            var version = _settings.GetValue<string>("Version", "undefined");
            var versionData = _settings.GetValue<string>("VersionData", "undefined");
            
            return new VersionModel()
            {
                Version = version,
                VersionData = versionData
            };
        }
    }
}
