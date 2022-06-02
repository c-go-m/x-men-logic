using System;
using System.Threading.Tasks;
using Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AppFunction.Common
{
    public class HealthValidation
    {
        private readonly HealthCheckService _healthCheck;

        public HealthValidation(HealthCheckService healthCheck)
        {
            _healthCheck = healthCheck;
        }

        [FunctionName(nameof(HealthCheck))]
        public async Task<IActionResult> HealthCheck(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Constants.VersionMicroservice + nameof(HealthCheck))]
        HttpRequest req)
        {
            var status = await _healthCheck.CheckHealthAsync();

            return new OkObjectResult(Enum.GetName(typeof(HealthStatus), status.Status));
        }
    }
}
