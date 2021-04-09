using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Andys.Function
{

    public static class ListEvents
    {


        public class Event
        {

            public string Name { get; set; }

        }



        [FunctionName("ListEvents")]
        public static List<String> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "Event",
                collectionName: "Events",
                ConnectionStringSetting = "CosmosK2DBConnection",
                SqlQuery = "select * from C")
            ]IEnumerable<Event> Events,
            ILogger log)
        {
            log.LogInformation($"Function triggered");
            List<string> EventList = new List<string>();

            if (Events == null)
            {
                log.LogInformation($"Function triggered");

            }
            else
            {
                var expenseItems = (List<Event>)Events;
                if (expenseItems.Count == 0)
                {
                    log.LogInformation($"No Todo items found");
                }
                else
                {
                    log.LogInformation("Todo items found");

                    foreach (var events in Events)
                    {
                        Console.WriteLine(events.Name);
                        EventList.Add(events.Name);
                    }


                  
                }
            }

            EventList.Add("Other");

            return EventList;


        }
    }
}