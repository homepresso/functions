using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Amazon.S3;
using Amazon.Runtime;
using Amazon.S3.Model;
using System.Collections.Generic;
using Andys.Function;

namespace Andys.Function
{
    public static class S3DeleteFolder
    {
        [FunctionName("S3DeleteFolder")]
        public static async Task<String> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            var bucketName = req.Query["bucketName"];
            var access = req.Headers["access"];
            var secret = req.Headers["secret"];
            var region = req.Query["region"];
            var folderPath = req.Query["folderPath"];

            var credentials = new BasicAWSCredentials(access, secret);
            var config = new AmazonS3Config

            {
                RegionEndpoint = andys.function.S3Region.getAWSRegion(region)
            };


            return null;
        }
    }
}
