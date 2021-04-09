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
{
    public class taskPayload

    {

        public string se_phonenumber { get; set; }
        public string se_response { get; set; }

    }

 
        public static class SFITwilioWebHook
    {


        [FunctionName("SFITwilioWebHook")]
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
                .Message($"Your SMS and response has been received. {formValues["Body"]}");
            var twiml = response.ToString();
            twiml = twiml.Replace("utf-16", "utf-8");
            var smsBody = formValues["Body"];
            var smsFrom = formValues["From"];
            log.LogInformation(smsBody);
            log.LogInformation(smsFrom);


            var taskPayLoad = new
            {
                se_response = smsBody,
                se_phonenumber = smsFrom
            };

            string json = JsonConvert.SerializeObject(taskPayLoad);
            StringContent taskdata = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "https://simpson.workflowcloud.com/api/v1/workflow/published/55abf3f0-d660-453d-8a7d-ee9c8dcc6918/instances?token=Dt6IwIAALWh4E5lbEh6g5eZQcwpcJLsaWp564FfLAm8mYd5Y2ONJDo6qZMO8dn7RzvmJtL";
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