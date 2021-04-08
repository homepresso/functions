using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DocumentFormat.OpenXml;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Linq;

namespace Andys.Function
{
    public static class ReadExcel
    {
        [FunctionName("ReadExcel")]
        public static DataTable Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            

            var file = req.Form.Files["file"];


            {
                try
                {
                    DataTable dtTable = new DataTable();
                    var filestream = file.OpenReadStream();
                    //Lets open the existing excel file and read through its content . Open the excel using openxml sdk
                    using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filestream, false))
                    {
                         
                        WorkbookPart workbookPart = doc.WorkbookPart;
                        Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();

                         
                        foreach (Sheet thesheet in thesheetcollection.OfType<Sheet>())
                        {
                              
                            Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;

                            SheetData thesheetdata = theWorksheet.GetFirstChild<SheetData>();



                            for (int rCnt = 0; rCnt < thesheetdata.ChildElements.Count(); rCnt++)
                            {
                                List<string> rowList = new List<string>();
                                for (int rCnt1 = 0; rCnt1
                                    < thesheetdata.ElementAt(rCnt).ChildElements.Count(); rCnt1++)
                                {

                                    Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rCnt).ChildElements.ElementAt(rCnt1);
                                     
                                    string currentcellvalue = string.Empty;
                                    if (thecurrentcell.DataType != null)
                                    {
                                        if (thecurrentcell.DataType == CellValues.SharedString)
                                        {
                                            int id;
                                            if (Int32.TryParse(thecurrentcell.InnerText, out id))
                                            {
                                                SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                                if (item.Text != null)
                                                {
                                                    
                                                    if (rCnt == 0)
                                                    {
                                                        dtTable.Columns.Add(item.Text.Text);
                                                    }
                                                    else
                                                    {
                                                        rowList.Add(item.Text.Text);
                                                    }
                                                }
                                                else if (item.InnerText != null)
                                                {
                                                    currentcellvalue = item.InnerText;
                                                }
                                                else if (item.InnerXml != null)
                                                {
                                                    currentcellvalue = item.InnerXml;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (rCnt != 0)
                                        {
                                            rowList.Add(thecurrentcell.InnerText);
                                        }
                                    }
                                }
                                if (rCnt != 0)
                                    dtTable.Rows.Add(rowList.ToArray());

                            }

                        }

                        //   return JsonConvert.SerializeObject(dtTable);
                        return dtTable;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
    }
}
