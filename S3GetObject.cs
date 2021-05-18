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
using System.Collections.Generic;
using Amazon.S3.Model;
using System.Text;

namespace Andys.Function
{
    public static class S3GetObject
    {
        [FunctionName("S3GetObject")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {


            var bucketName = req.Query["bucketName"];
            var access = req.Headers["access"];
            var secret = req.Headers["secret"];
            var region = req.Query["region"];
            var key = req.Query["key"];

            List<s3mod> filenames = new List<s3mod>();

            var credentials = new BasicAWSCredentials(access, secret);
            var config = new AmazonS3Config


            {
                RegionEndpoint = andys.function.S3Region.getAWSRegion(region)
            };

            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };
            using var client = new AmazonS3Client(credentials, config);

            using (GetObjectResponse response = await client.GetObjectAsync(request))
            {
                using (StreamReader reader = new StreamReader(response.ResponseStream))
                {
                    string contents = reader.ReadToEnd();
                    Console.WriteLine("Object - " + response.Key);
                    Console.WriteLine(" Version Id - " + response.VersionId);
                    Console.WriteLine(" Contents - " + contents);

                    byte[] filebytes = Encoding.UTF8.GetBytes(contents);

                    return new FileContentResult(filebytes, "application/octet-stream")
                    {
                        FileDownloadName = key

                    };

                }



            }
        }
    }
}
