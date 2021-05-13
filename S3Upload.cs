using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon;
using Amazon.Runtime;
using Microsoft.AspNetCore.Mvc;

namespace Andys.Function
{
    public static class S3Upload
    {
        [FunctionName("S3Upload")]
        public static async Task<String> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            var file = req.Form.Files["file"];
            var region = req.Query["region"];
            var bucket = req.Query["bucket"];
            var access = req.Headers["access"];
            var secret = req.Headers["secret"];


            return UploadS3File(bucket, region, access, secret, file).Result;
        }
        public static async Task<String> UploadS3File(string bucket, string region, string access, string secret, IFormFile file)
        {

            var credentials = new BasicAWSCredentials(access, secret);
            var config = new AmazonS3Config

            {
                RegionEndpoint = Amazon.RegionEndpoint.USWest2
            };
            using var client = new AmazonS3Client(credentials, config);
            await using var newMemoryStream = new MemoryStream();
            file.CopyTo(newMemoryStream);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = file.FileName,
                BucketName = bucket,
                CannedACL = S3CannedACL.PublicRead
            };

            var fileTransferUtility = new TransferUtility(client);
            await fileTransferUtility.UploadAsync(uploadRequest);

            return uploadRequest.Key;
        }

    }

   
}
   

