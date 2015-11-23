using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Documents;

namespace ContinuumDump.Common
{
    public class Wrapper
    {
        private Dictionary<string, ParamValue> _templateParameters = new Dictionary<string, ParamValue>();
        private ArrayList _templateLines = new ArrayList();
        private ObservableCollection<SingleDumpDataViewModel> csvDataCollectionVM = new ObservableCollection<SingleDumpDataViewModel>();
        private List<string> dataParam = new List<string>();
        private string _pattern = "&&.*?&&";

        public ObservableCollection<SingleDumpDataViewModel> CsvDataCollectionVM
        {
            get { return csvDataCollectionVM; }
        }

        public List<string> DataParam
        {
            get { return dataParam; }
        }

        public FlowDocument LoadTemplateDataAndReturnDocument(string FilePath)
        {
            TemplateHelper.ReadTemplate(FilePath, _pattern, _templateParameters, _templateLines);
            return TemplateHelper.CreateTemplateDocument(_pattern, _templateParameters, _templateLines);
        }

        public void LoadCsvData(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                csvDataCollectionVM.Clear();
                dataParam.Clear();
                using (StreamReader streamReader = new StreamReader(FilePath))
                {
                    string line;
                    Dictionary<string, string> singleDumpData = new Dictionary<string, string>();

                    if ((line = streamReader.ReadLine()) != null)
                    {
                        var values = line.Split(';');
                        foreach (string param in values)
                        {
                            dataParam.Add("&&" + param + "&&");
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
                        csvDataCollectionVM.Add(new SingleDumpDataViewModel(singleDumpModel));
                    }
                }
            }
        }

        public void GenerateDump(bool oneFile)
        {
            int i = 0;
            string fileExtension = Properties.Settings.Default.FileExtension;
            string fileNamePattern = "";
            fileNamePattern = fileNamePattern.PadRight(CsvDataCollectionVM.Count.ToString().Length , '0');
            foreach (SingleDumpDataViewModel single in CsvDataCollectionVM)
            {
                using (StreamWriter writer = new StreamWriter(string.Format("{0:"+ fileNamePattern + "}.{1}", i, fileExtension)))
                {
                    writer.Write(CreateOneLineToDump(single.CsvParam));
                }
                i++;
            }
        }

        private StringBuilder CreateOneLineToDump(IList<CSVParam> ParamToReplace)
        {
            StringBuilder stBuider = new StringBuilder();
            if (ParamToReplace != null)
            {
                foreach (string Line in _templateLines)
                {
                    string returnString = "";
                    MatchCollection matchReturn = Regex.Matches(Line, _pattern);
                    if (matchReturn.Count != 0)
                    {
                        int index = 0;
                        foreach (Match match in matchReturn)
                        {
                            returnString += Line.Substring(index, match.Index - index);
                            index = match.Index + match.Length;

                            CSVParam param = ParamToReplace.Where(x => x.Parameter == match.Value).FirstOrDefault();

                            if (param != null)
                                returnString += param.Value;
                            else
                                returnString += match.Value;
                        }
                        returnString += Line.Substring(index, Line.Length - index);
                    }
                    else
                    {
                        returnString = Line;
                    }
                    stBuider.AppendLine(returnString);
                }
                stBuider.Remove(stBuider.Length - 2, 2);
            }
            return stBuider;
        }
    }
}
