using DataAbstraction.EnvironmentVariables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BankCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EnvironmentExamles : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<EnvironmentExampleEntity> _options;

        public EnvironmentExamles(IConfiguration configuration, IOptions<EnvironmentExampleEntity> options)
        {
            _configuration = configuration;
            _options = options;
        }


        [HttpGet("values/GetValue")]
        [AllowAnonymous]
        public ActionResult GetValues()
        {
            EnvironmentExampleEntity example = new EnvironmentExampleEntity();

            example.MyBoolValue = _configuration.GetValue<bool>("MyEnvironmentValues:MyBoolValue");
            example.MyStringValue = _configuration.GetValue<string>("MyEnvironmentValues:MyStringValue");
            example.MyIntValue = _configuration.GetValue<int>("MyEnvironmentValues:MyIntValue");

            return Ok(example);
        }

        [HttpGet("values/GetSection/GetValue")]
        [AllowAnonymous]
        public ActionResult GetSection()
        {
            EnvironmentExampleEntity example = new EnvironmentExampleEntity();

            var settingsSection = _configuration.GetSection("MyEnvironmentValues");

            example.MyBoolValue = settingsSection.GetValue<bool>("MyBoolValue");
            example.MyStringValue = settingsSection.GetValue<string>("MyStringValue");
            example.MyIntValue = settingsSection.GetValue<int>("MyIntValue");

            return Ok(example);
        }

        [HttpGet("values/GetValuesWithOption")]
        [AllowAnonymous]
        public ActionResult GetValuesWithOption()
        {
            EnvironmentExampleEntity example = _options.Value;

            return Ok(example);
        }
    }
}
