using DataAbstraction.EnvironmentVariables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BankCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EnvironmentExamles : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<EnvironmentExampleEntity> _options;
        private IOptionsSnapshot<EnvironmentExampleEntitySecond> _snapshotOptions;

        public EnvironmentExamles(
            IConfiguration configuration, 
            IOptions<EnvironmentExampleEntity> options,
            IOptionsSnapshot<EnvironmentExampleEntitySecond> snapshotOptions
            )
        {
            _configuration = configuration;
            _options = options;
            _snapshotOptions = snapshotOptions;
        }


        [HttpGet("values/GetValue")]
        [AllowAnonymous]
        public ActionResult GetValues()//works like scoped
        {
            EnvironmentExampleEntity example = new EnvironmentExampleEntity();

            example.MyBoolValue = _configuration.GetValue<bool>("MyEnvironmentValues:MyBoolValue");
            example.MyStringValue = _configuration.GetValue<string>("MyEnvironmentValues:MyStringValue");
            example.MyIntValue = _configuration.GetValue<int>("MyEnvironmentValues:MyIntValue");

            return Ok(example);
        }

        [HttpGet("values/GetSection/GetValue")]
        [AllowAnonymous]
        public ActionResult GetSection()//works like scoped
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
        public ActionResult GetValuesWithOption()//works like singleton
        {
            EnvironmentExampleEntity example = _options.Value;

            return Ok(example);
        }

        [HttpGet("values/GetValuesBindedScoped")]
        [AllowAnonymous]
        public ActionResult GetValuesWithOptionScoped()//works like scoped
        {
            EnvironmentExampleEntitySecond example = new EnvironmentExampleEntitySecond();

            example.MyBoolValue = _snapshotOptions.Value.MyBoolValue;
            example.MyStringValue = _snapshotOptions.Value.MyStringValue;
            example.MyIntValue = _snapshotOptions.Value.MyIntValue;

            return Ok(example);
        }
    }
}
