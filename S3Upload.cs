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
using Andys.Function;



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
            var filename = req.Query["filename"];


            return UploadS3File(bucket, region, access, secret, file, filename).Result;
        }
        public static async Task<String> UploadS3File(string bucket, string region, string access, string secret, IFormFile file, string filename)
        {

            var credentials = new BasicAWSCredentials(access, secret);
            var config = new AmazonS3Config


            {
                RegionEndpoint = andys.function.S3Region.getAWSRegion(region)
            };
            using var client = new AmazonS3Client(credentials, config);
            await using var newMemoryStream = new MemoryStream();
            file.CopyTo(newMemoryStream);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = filename,
                BucketName = bucket,
                CannedACL = S3CannedACL.PublicRead
            };

            var fileTransferUtility = new TransferUtility(client);
            await fileTransferUtility.UploadAsync(uploadRequest);

            return uploadRequest.Key;
        }

    }

   
}
   

