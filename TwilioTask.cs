using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using Twilio.TwiML;
using Newtonsoft.Json;
using System.Net.Http;
using System.Linq;
using System.Runtime;
using System.Collections.Generic;


namespace Andys.Function

{ public class NWCPayload

    {

        public string number { get; set; }
        public string message { get; set; }
        public string id { get; set; }
        public string workflowID { get; set; }

    }
    public static class TwilioWebHook
    {


        [FunctionName("TwilioWebHook")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]  
            HttpRequestMessage req, ILogger log)
        {

            var data = await req.Content.ReadAsStringAsync();
            var formValues = data.Split('&')
                .Select(value => value.Split('='))
                .ToDictionary(pair => Uri.UnescapeDataString(pair[0]).Replace("+", " "),
                              pair => Uri.UnescapeDataString(pair[1]).Replace("+", " "));


            var response = new MessagingResponse()
                .Message($"Your SMS has been recieved, we will be in touch shortly with any follow up. {formValues["Body"]}");
            var twiml = response.ToString();
            twiml = twiml.Replace("utf-16", "utf-8");
            var smsBody = formValues["Body"];
            var smsFrom = formValues["From"];
            var smsID = formValues["MessageSid"];
            log.LogInformation(smsBody);
            log.LogInformation(smsFrom);
            log.LogInformation(smsID);


            var taskPayLoad = new
            {
                message = smsBody,
                number = smsFrom,
                id = smsID,
            };

            string json = JsonConvert.SerializeObject(taskPayLoad);
            StringContent taskdata = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "https://andys.azurewebsites.net/api/TwilioTask";
            var client = new HttpClient();

            var taskresponse = await client.PostAsync(url, taskdata);

            client.Dispose();


            return new HttpResponseMessage
            {
                Content = new StringContent(twiml, Encoding.UTF8, "application/xml")
            };

        
        }


    }
}
