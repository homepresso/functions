using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon;

namespace andys.function
{
    public static class S3Region
    {

        public static RegionEndpoint getAWSRegion(string region)
        {


            switch (region)
            {

                case "USWest2":
                    { return Amazon.RegionEndpoint.USWest2; }
                case "USWest1":
                    { return Amazon.RegionEndpoint.USWest1; }
                case "USEast1":
                    { return Amazon.RegionEndpoint.USEast1; }
                case "USEast2":
                    { return Amazon.RegionEndpoint.USEast2; }
                case "MESouth1":
                    { return Amazon.RegionEndpoint.MESouth1; }
                case "SAEast1":
                    { return Amazon.RegionEndpoint.SAEast1; }
                case "USGovCloudEast1":
                    { return Amazon.RegionEndpoint.USGovCloudEast1; }
                case "USGovCloudWest1":
                    { return Amazon.RegionEndpoint.USGovCloudWest1; }
                case "AFSouth1":
                    { return Amazon.RegionEndpoint.AFSouth1; }
                case "APEast1":
                    { return Amazon.RegionEndpoint.APEast1; }
                case "APNortheast1":
                    { return Amazon.RegionEndpoint.APNortheast1; }
                case "APNortheast2":
                    { return Amazon.RegionEndpoint.APNortheast2; }
                case "APNortheast3":
                    { return Amazon.RegionEndpoint.APNortheast3; }
                case "APSouth1":
                    { return Amazon.RegionEndpoint.APSouth1; }
                case "APSoutheast1":
                    { return Amazon.RegionEndpoint.APSoutheast1; }
                case "APSoutheast2":
                    { return Amazon.RegionEndpoint.APSoutheast2; }
                case "CACentral1":
                    { return Amazon.RegionEndpoint.CACentral1; }
                case "CNNorth1":
                    { return Amazon.RegionEndpoint.CNNorth1; }
                case "CNNorthWest1":
                    { return Amazon.RegionEndpoint.CNNorthWest1; }
                case "EUCentral1":
                    { return Amazon.RegionEndpoint.EUCentral1; }
                case "EUNorth1":
                    { return Amazon.RegionEndpoint.EUNorth1; }
                case "EUSouth1":
                    { return Amazon.RegionEndpoint.EUSouth1; }
                case "EUWest1":
                    { return Amazon.RegionEndpoint.EUWest1; }
                case "EUWest2":
                    { return Amazon.RegionEndpoint.EUWest2; }
                case "EUWest3":
                    { return Amazon.RegionEndpoint.EUWest3; }

            }
            return null;
        }
    }
}
