using ContinuumDump.Common;
using System.Collections.ObjectModel;

namespace ContinuumDump
{
    public class SingleDumpDataViewModel : Observable
    {
        private ObservableCollection<CSVParam> csvparam;

        public ObservableCollection<CSVParam> CsvParam
        {
            get { return csvparam; }
        }

        public SingleDumpDataViewModel(SingleDumpModel SingleDump)
        {
            csvparam = new ObservableCollection<CSVParam>(SingleDump.Parameters);
        }
    }
}
