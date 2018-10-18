using ClosedXML.Excel;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureMachineLearning
{
    public abstract class Exporter
    {
        public abstract bool Export(List<Model> data, string filename);
    }
    /// <summary>
    /// Exports an object to an excel file
    /// </summary>
    public class ExcelExporter : Exporter
    {
        public override bool Export(List<Model> data, string filename)
        {
            bool        Success = false;
            DataTable   dt      = new DataTable();
            XLWorkbook  wb      = new XLWorkbook();

            using (var reader = ObjectReader.Create(data))
            {
                dt.Load(reader);
            }

            wb.Worksheets.Add(dt, "Sheet 1");

            if (filename.Contains("."))
            {
                filename = filename.Substring(0, filename.LastIndexOf('.')) + ".xlsx";
            }

            filename = filename + ".xlsx";

            wb.SaveAs(filename);

            Success = true;

            return Success;
        }
    }
}
