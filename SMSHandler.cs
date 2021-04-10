using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Collections.Generic;
using Twilio.TwiML;
using System.Linq;
using System.Runtime.Serialization;
using System.Net;
using System.Runtime.Serialization.Json;

namespace Andys.Function
{
    public static class SMSHandler
    {

        [DataContract]
        public class DataFields
        {
            [DataMember(Name = "Body")]
            public string Body { get; set; }

            [DataMember(Name = "Number")]
            public string Number { get; set; }
        }

        [DataContract]
        class WorkflowInstance
        {
            [DataMember(Name = "expectedDuration")]
            public long ExpectedDuration { get; set; }
            [DataMember(Name = "folio")]
            public string Folio { get; set; }
            [DataMember(Name = "priority")]
            public long Priority { get; set; }
            [DataMember(Name = "dataFields")]
            public DataFields DataFields { get; set; }

        }

        public class ODataResponse2<T>
        {
            public List<T> Value { get; set; }
        }

        public class RecordGuid
        {
            public string ID { get; set; }
        }

 
        public class Records
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Phone_Number { get; set; }
            public string Country { get; set; }
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
            public string Company { get; set; }
            public string Platform { get; set; }
            public string NWCurl { get; set; }
            public string K2Url { get; set; }
            public string K2User { get; set; }
            public string K2Pass { get; set; }
            public DateTime Date_Created { get; set; }
            public DateTime Expiry_Date { get; set; }
            public string ReturnMessage { get; set; }
            public string Used { get; set; }
            public string Status { get; set; }
        }

        public class Root
        {
            [JsonProperty("@odata.context")]
            public string OdataContext { get; set; }
            [JsonProperty("@odata.count")]
            public int OdataCount { get; set; }
            public List<Records> value { get; set; }
        }

     


        [FunctionName("SMSHandler")]
        public static async Task<Object> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage reqm, HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Main Request");
            string GUID = req.Query["GUID"];
            log.LogInformation(GUID);
            Records RecordsObj = (Records)await GetRecord(GUID, new Records());



            var data = await reqm.Content.ReadAsStringAsync();
            var formValues = data.Split('&')
                .Select(value => value.Split('='))
                .ToDictionary(pair => Uri.UnescapeDataString(pair[0]).Replace("+", " "),
                              pair => Uri.UnescapeDataString(pair[1]).Replace("+", " "));

            var response = new MessagingResponse()
                .Message($"Your Message {formValues["Body"]}");
            var twiml = response.ToString();
            twiml = twiml.Replace("utf-16", "utf-8");
            var smsBody = formValues["Body"];
            var smsFrom = formValues["From"];
            var smsID = formValues["MessageSid"];
            var smsDate = DateTime.Now;
            string dateString = smsDate.ToString();
            string platform = RecordsObj.Platform;
            log.LogInformation(platform);

            if (platform == "NWC" || platform == "nwc" || platform == "Nwc")
            {
                string NWCurl = RecordsObj.NWCurl;
     

                var NWCPayLoad = new
                {
                    se_number = smsFrom,
                    se_body = smsBody
                };

                string NWCTaskjson = JsonConvert.SerializeObject(NWCPayLoad);
                StringContent nwctaskdata = new StringContent(NWCTaskjson, Encoding.UTF8, "application/json");

                var client = new HttpClient();

                var taskresponse = await client.PostAsync(NWCurl, nwctaskdata);

                client.Dispose();

                return RecordsObj.ReturnMessage;
            }


            if (platform == "K2" || platform == "k2")

            {
                string K2Url = RecordsObj.K2Url;
                log.LogInformation(K2Url);
                string K2User = RecordsObj.K2User;
                log.LogInformation(K2User);
                string K2Pass = RecordsObj.K2Pass;
                log.LogInformation(K2Pass);

                log.LogInformation("C# HTTP trigger function processed a request.");
                HttpClient k2CloudClient;
                NetworkCredential k2credentials = new NetworkCredential(K2User, K2Pass);
                HttpClientHandler loginHandler = new HttpClientHandler
                {
                    Credentials = k2credentials
                };

                k2CloudClient = new HttpClient(loginHandler, true);

                string requesturi = K2Url;

                string instanceDataJSON;
                string body = smsBody;
                string number = smsFrom;

                DataFields myDF = new DataFields();
                myDF.Body = body;
                myDF.Number = number;


                WorkflowInstance myInstance = new WorkflowInstance()
                {
                    Folio = "Process started via Twilio SMS",
                    Priority = 1,
                    DataFields = myDF
                };


                using (MemoryStream ms = new MemoryStream())
                {
                    DataContractJsonSerializer mySerializerWI = new DataContractJsonSerializer(myInstance.GetType());
                    mySerializerWI.WriteObject(ms, myInstance);
                    StreamReader reader = new StreamReader(ms);
                    ms.Position = 0;
                    instanceDataJSON = reader.ReadToEnd();
                }

                StringContent datacontent = new StringContent(instanceDataJSON, Encoding.UTF8, "application/json");
                var task = k2CloudClient.PostAsync(requesturi, datacontent);

                HttpResponseMessage myResponse = task.Result;

                return RecordsObj.ReturnMessage;
            }


            else

            {
                return "Missing Platform";
            }


        }

        static string TestMessage(string message)
        {

            return message;
        }

        static async Task<Object> GetRecord(string GUID, Records recObj)

        {

            using (var client = new HttpClient())
            {

                var url = $"https://hss1myd.onk2.com/api/odatav4/v4/Records_1?$filter=startswith(GUID, '{GUID}')";
                var Username = "administrator@immersiononk2us.com";
                var Password = "Immerse123!!";
                var authToken = Encoding.ASCII.GetBytes($"{Username}:{Password}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                HttpResponseMessage response = await client.GetAsync(new Uri(url));
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ODataResponse2<Records>>(json);

                var records = result.Value;

                foreach (var item in records)

                {
                    recObj.ID = item.ID;
                    recObj.First_Name = item.First_Name;
                    recObj.Name = item.Name;
                    recObj.Phone_Number = item.Phone_Number;
                    recObj.Country = item.Country;
                    recObj.Last_Name = item.Last_Name;
                    recObj.Company = item.Company;
                    recObj.Platform = item.Platform;
                    recObj.NWCurl = item.NWCurl;
                    recObj.K2Url = item.K2Url;
                    recObj.K2User = item.K2User;
                    recObj.K2Pass = item.K2Pass;
                    recObj.Date_Created = item.Date_Created;
                    recObj.Expiry_Date = item.Expiry_Date;
                    recObj.Used = item.Used;
                    recObj.Status = item.Status;
                    recObj.ReturnMessage = item.ReturnMessage;
                   
                }

                return recObj;

            }
        }
    }
}
