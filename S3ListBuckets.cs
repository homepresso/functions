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

namespace functions
{
    public static class S3ListBuckets
    {
        [FunctionName("S3ListBuckets")]
        public static async Task<List<string>> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {


            string access = req.Headers["access"];
            string secret = req.Headers["secret"];

            List<string> bucketnames = new List<string>();

            var credentials = new BasicAWSCredentials(access, secret);
            var config = new AmazonS3Config

            {
                RegionEndpoint = Amazon.RegionEndpoint.USWest2
            };
            using var client = new AmazonS3Client(credentials, config);

            ListBucketsResponse buckets = await client.ListBucketsAsync();
            foreach (var bucket in buckets.Buckets)
            { 
                bucketnames.Add(bucket.BucketName);

            }

            return bucketnames;
        }
    }
}
