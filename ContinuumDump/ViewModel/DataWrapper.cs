using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContinuumDump
{
    public class DataToReplaceWrapper : INotifyPropertyChanged
    {
        private ObservableCollection<SingleDumpDataViewModel> dumpDataCollectionVM = new ObservableCollection<SingleDumpDataViewModel>();
        private List<string> dataParam = new List<string>();

        public ObservableCollection<SingleDumpDataViewModel> DumpDataCollectionVM
        {
            get { return dumpDataCollectionVM; }
        }

        public List<string> DataParam
        {
            get { return dataParam; }
        }

        public void LoadDumpData(String FilePath)
        {
            if (File.Exists(FilePath))
            {
                dumpDataCollectionVM.Clear();
                dataParam.Clear();
                using (StreamReader streamReader = new StreamReader(FilePath))
                {
                    string line;
                    Dictionary<string, string> singleDumpData = new Dictionary<string, string>();

                    if ((line = streamReader.ReadLine()) != null)
                    {
                        var values = line.Split(';');
                        foreach(string param in values)
                        {
                            dataParam.Add("&&"+param+"&&");
                        }
                    }

                    SingleDumpModel singleDumpModel;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        int iii = 0;
                        var values = line.Split(';');
                        foreach (string param in values)
                        {
                            singleDumpData[dataParam[iii]] = param;
                            iii++;
                        }                                         

                        singleDumpModel = new SingleDumpModel(singleDumpData);
                        dumpDataCollectionVM.Add(new SingleDumpDataViewModel(singleDumpModel));
                    }
                }
            }
        }

        public void GenerateDump(DataTemplate dataTemplate, bool oneFile)
        {
            int i = 0;

            foreach (SingleDumpDataViewModel single in DumpDataCollectionVM)
            {

                using (StreamWriter writer = new StreamWriter(i + ".dmp"))
                {
                    writer.Write(dataTemplate.CreateOneLineToDump(single.CsvParam));
                }
                i++;
            }
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
