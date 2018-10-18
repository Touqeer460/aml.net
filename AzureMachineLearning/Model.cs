using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureMachineLearning
{
    public class Model
    {
        public string Item      { get; set; }
        public string Name      { get; set; }
        public double Quantity  { get; set; }
        public int    ShelfLife { get; set; }
    }
    public class Value
    {
        public List<string> ColumnNames { get; set; }
        public List<string> ColumnTypes { get; set; }
        public List<List<string>> Values { get; set; }
    }

    public class Output2
    {
        public string type { get; set; }
        public Value value { get; set; }
    }

    public class Value2
    {
        public List<string> ColumnNames { get; set; }
        public List<string> ColumnTypes { get; set; }
        public List<List<string>> Values { get; set; }
    }

    public class Output1
    {
        public string type { get; set; }
        public Value2 value { get; set; }
    }

    public class Results
    {
        public Output2 Output2 { get; set; }
        public Output1 Output1 { get; set; }
    }

    public class RootObject
    {
        public Results Results { get; set; }
        public static RootObject GetResults(string jsonStr)
        {
            Newtonsoft.Json.JsonSerializerSettings setting = new Newtonsoft.Json.JsonSerializerSettings();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(jsonStr);
        }
    }
}
