using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuumDump
{
    public class SingleDumpDataViewModel : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
