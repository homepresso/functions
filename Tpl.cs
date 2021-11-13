using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Andys.Function
{

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Root
    {
        public string ASIN { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string listPrice { get; set; }
        public string imageUrl { get; set; }
        public string detailPageURL { get; set; }
        public string rating { get; set; }
        public string totalReviews { get; set; }
        public string subtitle { get; set; }
        public string isPrimeEligible { get; set; }
    }


    public static class Tpl
    {
        [FunctionName("Tpl")]
        public static List<Root> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            var JobID = req.Headers["JobID"];

            Console.WriteLine(JobID);


     //       Root r = new Root

      List<Root> Products =  new List<Root>();
            
            Products.Add(new Root{

        ASIN = "B07RQ3NL76",
        title = "Gaggia RI9380/46 Classic Pro Espresso Machine, Solid, Brushed Stainless Steel",
        price = "$446.85",
        listPrice = "",
        imageUrl = "https://m.media-amazon.com/images/I/41FDt+X6kUL._SL160_.jpg",
        detailPageURL = "https://www.amazon.com/dp/B07RQ3NL76",
        rating = "4.6",
        totalReviews = "1331",
        subtitle = "Gaggia",
        isPrimeEligible = "1",


            });

                        Products.Add(new Root{

        ASIN = "B01G82WVZ0",
        title = "Baratza Sette 270 Conical Burr Coffee Grinder",
        price = "$399.95",
        listPrice = "",
        imageUrl =  "https://m.media-amazon.com/images/I/41leGjOtouL._SL160_.jpg",
        detailPageURL =  "https://www.amazon.com/dp/B01G82WVZ0",
        rating =  "4.4",
        totalReviews = "405",
        subtitle = "Baratza",
        isPrimeEligible = "1"


            });
            
        



            return Products;
        }
    }
}
