using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContinuumDump
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataTemplate dataTemplate = new DataTemplate();
        private DataToReplaceWrapper dataWrapper = new DataToReplaceWrapper();

        public MainWindow()
        {
            InitializeComponent();
            dgData.DataContext = dataWrapper;
        }

        private string OpenFileDialog(string DefaultName, string Extension, string Filter)
        {
            string filename = "";
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = DefaultName; 
            dlg.DefaultExt = Extension; 
            dlg.Filter = Filter;  

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                filename = dlg.FileName;
            }
            return filename;
        }

        private void FillDataGrid()
        {
            dgData.Columns.Clear();
            int i = 0;
            foreach(string param in dataWrapper.DataParam)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Header = param;
                column.Binding = new Binding("CsvParam["+i+"].Value");
                dgData.Columns.Add(column);
                i++;
            }
        }

        private void btImportTemplate_Click(object sender, RoutedEventArgs e)
        {
            dataTemplate.LoadTemplateFromFile(OpenFileDialog("Template", ".dmp", "Template (.dmp)|*.dmp"));
            dataTemplate.FillRichTextBox(rtbTemplateText);
        }

        private void btImportCSV_Click(object sender, RoutedEventArgs e)
        {
            dataWrapper.LoadDumpData(OpenFileDialog("Data", ".csv", "CSV  (.csv)|*.csv"));
            FillDataGrid();
        }

        private void btGenerateDumps_Click(object sender, RoutedEventArgs e)
        {
            dataWrapper.GenerateDump(dataTemplate, true);
        }
    }
}
