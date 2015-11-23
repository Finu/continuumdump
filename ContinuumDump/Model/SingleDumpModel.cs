using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuumDump
{
    public class SingleDumpModel
    {
        private List<CSVParam> parameters = new List<CSVParam>();

        public List<CSVParam> Parameters
        {
            get {return parameters;}
        }

        public SingleDumpModel(Dictionary<string, string> DataParam)
        {
            if (DataParam!=null)
            {
                foreach(var pair in DataParam)
                {
                    parameters.Add(new CSVParam(pair.Key, pair.Value));
                }
            }
        }
    }
}
