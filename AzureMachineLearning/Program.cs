using AzureMachineLearning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using ClosedXML.Excel;
using System.Data;
using FastMember;
using System.Configuration;

namespace CallRequestResponseService
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Model>         items       = new List<Model>();
            Exporter            exporter    = new ExcelExporter();
            AppSettingsReader   appSettings = new AppSettingsReader();
            DataHelper          dataHelper  = new DataHelper();
            
            /*
             * Sends request to azure server by collecting data from SQL Server.
             */
            RootObject outcome = processRequest(
                (string)appSettings.GetValue("AzureServiceUrl", typeof(string)),
                (string)appSettings.GetValue("AzureAuthenticationToken", typeof(string)),
                System.IO.File.ReadAllText((string)appSettings.GetValue("AzureInputFileTemplate", typeof(string))).
                    Replace("{enter}", "\n").Replace("{values}", string.Join(",", dataHelper.GetSqlDataFromAxDB().ToArray())));
            
            /*
             * Exports the model to Excel File.
             */

            if (outcome != null && outcome.Results != null 
                    && outcome.Results.Output1 != null && outcome.Results.Output1.value != null)
            {
                outcome.Results.Output1.value.Values.ToList().GroupBy(x => x[0]).ToList().ForEach(x => 
                {
                    INVENTTABLE item = dataHelper.GetItem(x.ToArray()[0][0]);
                    items.Add(new Model() 
                    { 
                        Item = x.ToArray()[0][0],
                        Name = dataHelper.GetName(x.ToArray()[0][0]), 
                        Quantity = Convert.ToDouble(x.ToArray()[0][2]),
                        ShelfLife = item.PDSSHELFLIFE
                        
                    });
                });
            }

            exporter.Export(items, (string)appSettings.GetValue("OutputExcelFileName", typeof(string)));
        }

        public static RootObject processRequest(string Url, string AuthenticationToken, string jsonData)
        {
            var request = (HttpWebRequest)WebRequest.Create(Url);
            var postData = jsonData;
            var data = Encoding.ASCII.GetBytes(postData);
            request.Headers.Add("Authorization", string.Format("Bearer {0}", AuthenticationToken));
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("Request failed. Status Code {0}\n", response.StatusCode);

            }
            return RootObject.GetResults(responseString);
        }
        
    }
}