using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;


namespace Andys.Function
{
    public static class UploadToBlob
    {
        [FunctionName("UploadToBlob")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            {
                try
                {
                    var formdata = await req.ReadFormAsync();
                    string connectionString = req.Headers["ConnString"];
                    string GUIDfilename = req.Query["BuildGUID"];
                    var file = req.Form.Files["file"];
                    string blobName = file.FileName;
                    string containername = req.Query["container"];
                    bool useGUID = Convert.ToBoolean(GUIDfilename);

                    if (useGUID == true)
                        {
                        string GUIDName = Guid.NewGuid().ToString();
                        blobName = GUIDName + blobName;
                        }
                    
                    BlobContainerClient container = new BlobContainerClient(connectionString, containername);
                    BlobClient blob = container.GetBlobClient(blobName);
                    var filestream = file.OpenReadStream();
                    await blob.UploadAsync(filestream);


                    return new OkObjectResult(blobName + " - " + file.Length.ToString());
                }

                catch (Exception ex)
                {
                    return new BadRequestObjectResult(ex);
                }
            }

        }
    }
}

       
    





