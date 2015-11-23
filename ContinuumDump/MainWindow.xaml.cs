using ContinuumDump.Common;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ContinuumDump
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Wrapper wrapper = new Wrapper();

        public MainWindow()
        {
            InitializeComponent();
            dgData.DataContext = wrapper;
        }

        private string OpenFileDialog(string DefaultName, string Extension, string Filter)
        {
            string filename = "";
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = DefaultName; 
            dlg.DefaultExt = Extension; 
            dlg.Filter = Filter;

            bool? result = dlg.ShowDialog();

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
            foreach(string param in wrapper.DataParam)
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
            string filePath = OpenFileDialog("Template", ".dmp", "Template (.dmp)|*.dmp");
            rtbTemplateText.Document = wrapper.LoadTemplateDataAndReturnDocument(filePath);
        }

        private void btImportCSV_Click(object sender, RoutedEventArgs e)
        {
            string filePath = OpenFileDialog("Data", ".csv", "CSV  (.csv)|*.csv");
            wrapper.LoadCsvData(filePath);
            FillDataGrid();
        }

        private void btGenerateDumps_Click(object sender, RoutedEventArgs e)
        {
            wrapper.GenerateDump(true);
        }
    }
}
