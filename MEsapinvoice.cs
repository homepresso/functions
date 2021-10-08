using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;
using System.Text.Json;



namespace Andys.Function
{
    public static class MEsapinvoice
    {

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class InvoiceItemSet
    {
        public string ItemCode { get; set; }
        public string Qty { get; set; }
        public string PriceEach { get; set; }
        public string Amount { get; set; }
        public string DocumentCurr { get; set; }
        public string ITEM_DESCR { get; set; }
    }

    public class Root
    {
        public string DueDate { get; set; }
        public string Invoice { get; set; }
        public string InvoiceDate { get; set; }
        public string Po { get; set; }
        public string SubTotalAmount { get; set; }
        public string TaxAmount { get; set; }
        public string GROSS_AMOUNT { get; set; }
        public string Message { get; set; }
        public string CURRENCY { get; set; }
        public List<InvoiceItemSet> InvoiceItemSet { get; set; }
    }


        [FunctionName("MEsapinvoice")]
        public static async Task<dynamic> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            var userName = "Nintex";
            var password = "Welcome1";


                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };


            var url = "https://34.201.227.223:44300/sap/opu/odata/sap/ZINVOICE_POST_SRV/InvoiceCreateSet?sap-client=599";
            
            
            using var client = new HttpClient(clientHandler);
                var authToken = Encoding.ASCII.GetBytes($"{userName}:{password}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(authToken));

            {

                var response = await client.GetAsync(url);
                client.DefaultRequestHeaders.Add("x-CSRF-token", "fetch");

            }

            var result = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

            var csrftoken = result.Headers.GetValues("x-CSRF-token").FirstOrDefault();

            using var postClient = new HttpClient(clientHandler);



      string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      dynamic data = JsonConvert.DeserializeObject(requestBody);

                string jsondata = JsonConvert.SerializeObject(data);
                StringContent bodydata = new StringContent(jsondata, Encoding.UTF8, "application/json");

            {
                var postresponse = await client.PostAsync(url, bodydata);
                postClient.DefaultRequestHeaders.Add("X-CSRF-Token", csrftoken);

            }



return postClient;
        }
    }
}
