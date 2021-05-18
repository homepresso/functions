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

namespace Andys.Function
{
    public static class S3CreateObject
    {
        [FunctionName("S3CreateObject")]
        public static async Task<String> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = null)] HttpRequest req,
            ILogger log)
        {

            var bucketName = req.Query["bucketName"];
            var access = req.Headers["access"];
            var secret = req.Headers["secret"];
            var key = req.Query["key"];
            var region = req.Query["region"];
            var contentBody = req.Form["contentBody"];

            Console.WriteLine(contentBody);
            Console.WriteLine("g");

            var credentials = new BasicAWSCredentials(access, secret);
            var config = new AmazonS3Config



            {
                RegionEndpoint = andys.function.S3Region.getAWSRegion(region)
            };

           
                using var client = new AmazonS3Client(credentials, config);

            // Create a PutObject request
            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = "nintexna",
                Key = "Item1",
                ContentType = "text/plain",
                ContentBody = "This is sample content..."
            };

            // Put object
            PutObjectResponse response = await client.PutObjectAsync(request);






            return contentBody.ToString();
        }

    }
}
