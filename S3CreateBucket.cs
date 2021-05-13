using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.Runtime;
using Amazon.S3.Model;

namespace functions
{
    public static class S3CreateBucket
    {
        [FunctionName("S3CreateBucket")]
        public static async Task<String> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            var bucketName = req.Query["bucketName"];
            var access = req.Headers["access"];
            var secret = req.Headers["secret"];


            {
                var credentials = new BasicAWSCredentials(access, secret);
                var config = new AmazonS3Config

                {
                    RegionEndpoint = Amazon.RegionEndpoint.USWest2
                };
                using var client = new AmazonS3Client(credentials, config);
                try
                {
                    PutBucketRequest request = new PutBucketRequest();
                    request.BucketName = bucketName;
                    client.PutBucketAsync(request);
                }
                catch (AmazonS3Exception amazonS3Exception)
                {
                    if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        Console.WriteLine("Incorrect AWS Credentials.");
                    }
                    else
                    {
                        Console.WriteLine("Error: ", amazonS3Exception.ErrorCode, amazonS3Exception.Message);
                    }
                }
            }


            return bucketName;
        }
    }
}
