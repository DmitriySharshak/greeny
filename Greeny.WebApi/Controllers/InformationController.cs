using Microsoft.AspNetCore.Mvc;

namespace Greeny.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/info")]
    [Produces("application/json")]
    public class InformationController
    {
        private readonly IConfiguration _settings;

        public InformationController(IConfiguration settings)
        {
            _settings = settings;
        }

        [HttpGet("version")]
        public string GetApiVersion()
        {
            var version = _settings.GetValue<string>("Version", "v1");
            return $"Версия сервера: {version}";
        }
    }
}
