using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Andys.Function
{

        public class Results
    {
        public int? TaxID { get; set; }
        public double ReliabilityRating { get; set; }
        public string CurrentBankrupt { get; set; }
        public string PreviousBankrupt { get; set; }

    }
    public static class experian
    {
        [FunctionName("experian")]
        public static async Task<Results> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trgdfgfddfgigg1er f request.");

            string name = req.Query["Businessname"];
            string city = req.Query["City"];
            string country = req.Query["Country"];

            Results r = new Results 

            {

                TaxID = 0004121512,
                ReliabilityRating = 71.4,
                CurrentBankrupt = "No",
                PreviousBankrupt = "No"

            };


            return r;
        }
    }
}
