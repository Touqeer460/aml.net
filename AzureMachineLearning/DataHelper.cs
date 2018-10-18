using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureMachineLearning
{
    public class DataHelper
    {
        static DateTime minDate = new DateTime(1900, 01, 01);
        static MicrosoftDynamicsAXEntities context = new MicrosoftDynamicsAXEntities();
        public string GetName(string ItemId)
        {
            long product = context.INVENTTABLEs.Where(x => x.ITEMID == ItemId).First().PRODUCT;
            long recId = context.ECORESPRODUCTs.Where(x => x.RECID == product).First().RECID;
            return context.ECORESPRODUCTTRANSLATIONs.Where(x => x.PRODUCT == recId).First().NAME;
        }

        public INVENTTABLE GetItem(string ItemId)
        {
            return context.INVENTTABLEs.Where(r => r.ITEMID == ItemId).First();
        }

        /// <summary>
        /// Get Data from usmf company and formalize it as parsable input for the web service
        /// </summary>
        /// <returns>
        /// Returns the formalized data from sql
        /// </returns>
        public List<string> GetSqlDataFromAxDB()
        {
            List<string> data = new List<string>();
            List<INVENTTRAN> inv = context.INVENTTRANS.Where(x => x.DATAAREAID == "usmf" && x.QTY < 0).ToList().Take(2000).ToList();
            inv.ForEach(x =>
            {
                data.Add(string.Format("[ \"{0}\", \"{1}\", \"{2}\"]", x.ITEMID, DateToDateKey(x.MODIFIEDDATETIME), Math.Abs(x.QTY)));
            });
            return data;
        }

        public long DateToDateKey(DateTime dt)
        {
            return (long)((dt - minDate).Days * 0.031);
        }
    }
}
