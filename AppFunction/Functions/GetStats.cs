
using BusinessLogic.Interfaces;
using Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Threading.Tasks;

namespace AppFunction.Functions
{
    public class GetStats
    {
        private readonly IDnaMutant dnaMutant;
        public GetStats(IDnaMutant dnaMutant)
        {
            this.dnaMutant = dnaMutant;
        }

        [FunctionName("stats")]
        public async Task<IActionResult> StatsAsync(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Constants.VersionMicroservice + Constants.Stats)] HttpRequest req)
        {
            try
            {
                var result = await dnaMutant.StatsAsync();

                return new OkObjectResult(result);                                
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);

            }
        }
    }
}
