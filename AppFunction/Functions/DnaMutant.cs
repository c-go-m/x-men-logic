using BusinessLogic.Interfaces;
using Common.Constants;
using Entities.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AppFunction.Functions
{
    public class DnaMutant
    {
        private readonly IDnaMutant dnaMutant;
        public DnaMutant(IDnaMutant dnaMutant)
        {
            this.dnaMutant = dnaMutant;
        }

        [FunctionName("mutant")]
        public async Task<IActionResult> ValidDnaAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Constants.VersionMicroservice + Constants.IsMutant)][FromBody] Petition petition
            , ILogger log)
        {
            try
            {
                var result = await dnaMutant.ValidDNA(petition.dna);

                if (result) {                 
                    return new OkObjectResult(result);
                }

                return new StatusCodeResult((int)HttpStatusCode.Forbidden);

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
                
            }
        }
    }
}
