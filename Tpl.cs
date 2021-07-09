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
    public class ShipmentDetail
    {
        public int? SalesOrderId { get; set; }
        public string SalesOrderNumber { get; set; }
        public string PlannedShipmentNbr { get; set; }
        public string InvoiceNumber { get; set; }
        public object InvoiceDateUtc { get; set; }
        public double? PreTaxAmount { get; set; }
        public double? TaxAmount { get; set; }
        public double? InvoiceAmount { get; set; }
        public string InvoiceStatus { get; set; }
    }

    public class Document
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }

    public class Root
    {
        public int? CreditJobId { get; set; }
        public string TaggedTransaction { get; set; }
        public string OracleProjectIndicator { get; set; }
        public string R12ProjectNumber { get; set; }
        public int? ChangeOrderId { get; set; }
        public string ChangeOrderGuid { get; set; }
        public int? ChangeOrderTypeId { get; set; }
        public string ChangeOrderChangeType { get; set; }
        public int? ChangeOrderTypeSequence { get; set; }
        public string CreditReasonCode { get; set; }
        public bool? IsRequiredCreditRebill { get; set; }
        public bool? IsRequiredCreditAllInvoices { get; set; }
        public bool? IsRequiredDeliverCreditMemos { get; set; }
        public bool? IsRequiredDeliverNewInvoices { get; set; }
        public string WorkflowNote { get; set; }
        public string TransmittedBy { get; set; }
        public string OriginalCustomerAccount { get; set; }
        public string CreditJobNumber { get; set; }
        public double? POTotal { get; set; }
        public string OriginalPONumber { get; set; }
        public bool? IsRequiredTaxReview { get; set; }
        public string SalesOfficeCode { get; set; }
        public string SalesOfficeName { get; set; }
        public List<ShipmentDetail> ShipmentDetails { get; set; }
        public string InvoiceMethod { get; set; }
        public int? OriginalShippingId { get; set; }
        public int? NewShippingId { get; set; }
        public string OldShippingAddress { get; set; }
        public string NewShippingAddress { get; set; }
        public List<Document> Documents { get; set; }
        public bool? IsFinalFinisher { get; set; }
        public string CorrelationId { get; set; }
        public string ParentId { get; set; }
        public string StepId { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string SourceApplication { get; set; }
        public string Status { get; set; }
        public object Comments { get; set; }
        public object AdditionalInfo { get; set; }
        public object Signature { get; set; }
        public bool? CanAutoClose { get; set; }
        public string Tag { get; set; }
        public DateTime? CreatedOn { get; set; }
        public object ClosedOn { get; set; }
    }


    public static class Tpl
    {
        [FunctionName("Tpl")]
        public static Root Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            var JobID = req.Headers["JobID"];

            Console.WriteLine(JobID);

            Root r = new Root
            {

                CreditJobId = Int32.Parse(JobID),
                TaggedTransaction = "Ship To",
                OracleProjectIndicator = "Y",
                R12ProjectNumber = "US-F164849",
                ChangeOrderId = 0,
                ChangeOrderGuid = "4b3993c1-220e-489a-9f93-cff408c7de8d",
                ChangeOrderTypeId = 2394764,
                ChangeOrderChangeType = "Ship To",
                ChangeOrderTypeSequence = 1,
                CreditReasonCode = "CREDIT REBILL",
                IsRequiredCreditRebill = true,
                IsRequiredCreditAllInvoices = false,
                IsRequiredDeliverCreditMemos = false,
                IsRequiredDeliverNewInvoices = true,
                WorkflowNote = "UAT testing for CPO",
                TransmittedBy = "ccebbz",
                OriginalCustomerAccount = "6084933",
                CreditJobNumber = "F164849",
                POTotal = 62825,
                OriginalPONumber = "4818-R",
                IsRequiredTaxReview = false,
                SalesOfficeCode = "F1",
                SalesOfficeName = "Charlotte",
                ShipmentDetails = new List<ShipmentDetail>(){new ShipmentDetail{SalesOrderId = 3492008, SalesOrderNumber = "F1c849", PlannedShipmentNbr = "B", InvoiceNumber = "1000525", PreTaxAmount = 25689.4, TaxAmount = 1862.49, InvoiceAmount = 27551.89, InvoiceStatus = "Completed"}
                , new ShipmentDetail{SalesOrderId = 3492008, SalesOrderNumber = "F1c849", PlannedShipmentNbr = "C", InvoiceNumber = "1000525", PreTaxAmount = 25689.4, TaxAmount = 1862.49, InvoiceAmount = 27551.89, InvoiceStatus = "Completed"}},
                InvoiceMethod = "CPO",
                OriginalShippingId = 2,
                NewShippingId = 1,
                OldShippingAddress = "AME Crane Service\nc/o United MEch - Huntersville\n4517 Equipment Drive\nCHARLOTTE, NC, 28269\nMECKLENBURG\nUS",
                NewShippingAddress = "UNITED MECHANICAL CORPORATION\n2811 CENTRAL AVENUE\nAttn: Parker \nCHARLOTTE, NC, 28205\nMECKLENBURG\nUS",
                Documents = new List<Document>(){new Document{FileName = "Doc.pdf", FilePath = "Jobs/CreditProjects/2394764/ChangeOrders/4b3993c1-220e-489a-9f93-cff408c7de8d/ProofOfDelivery/Doc.pdf"}, new Document{FileName = "FY21_COVID19_VAR_InStore_ENG.pdf", FilePath = "Jobs/CreditProjects/2394764/ChangeOrders/4b3993c1-220e-489a-9f93-cff408c7de8d/FinalFinisher/FY21_COVID19_VAR_InStore_ENG.pdf"}},
                IsFinalFinisher = true,
                CorrelationId = "aae23d22-3422-49b1-91e1-a69f4b81b068",
                ParentId = "7206da18-d809-4a38-ab47-656a411b95c0",
                StepId = "0aa91ca8-ef21-47a4-ba8b-7cedfa7657de",
                EntityName = "CreditJob",
                EntityId = "2394764",
                SourceApplication = "BILLING",
                Status = "Initiated",
                CanAutoClose = false,
                Tag = "ShipTo",
                CreatedOn = DateTime.Parse("2021-07-08T15:40:35.5310888Z")

            };



            return r;
        }
    }
}
