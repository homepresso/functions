using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System.Collections.Generic;

namespace functions
{
    public static class S3ListFiles
    {
        [FunctionName("S3ListFiles")]
        public static async Task<String> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string bucketName = req.Query["bucketName"];
            var folder = req.Query["folder"];
            var access = req.Headers["access"];
            var secret = req.Headers["secret"];
            var region = req.Query["region"];
            var Prefix = req.Headers["prefix"];

            List<string> filenames = new List<string>();

            var credentials = new BasicAWSCredentials(access, secret);
            var config = new AmazonS3Config


            {
                RegionEndpoint = Amazon.RegionEndpoint.USWest2
            };
            using var client = new AmazonS3Client(credentials, config);

            ListObjectsRequest request = new ListObjectsRequest();
            request.BucketName = bucketName;
            ListObjectsResponse response = await client.ListObjectsAsync(request);
            foreach (S3Object o in response.S3Objects)
            {
                Console.WriteLine("{0}\t{1}\t{2}", o.Key, o.Size, o.LastModified);
            }

            return null;
        }
    }
}
