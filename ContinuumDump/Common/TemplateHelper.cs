using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ContinuumDump.Common
{
    public class TemplateHelper
    {
        public static void ReadTemplate(string FilePath, string pattern, Dictionary<string, ParamValue> parameters, ArrayList lines)
        {
            parameters.Clear();
            lines.Clear();
            if (File.Exists(FilePath))
            {
                using (StreamReader streamReader = new StreamReader(FilePath))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        foreach (Match match in Regex.Matches(line, pattern))
                        {
                            if (!parameters.ContainsKey(match.Value))
                                parameters.Add(match.Value, new ParamValue(match.Value));
                        }
                        lines.Add(line);
                    }
                }
            }
        }

        public static FlowDocument CreateTemplateDocument(string pattern, Dictionary<string, ParamValue> parameters, ArrayList lines)
        {
            FlowDocument document = new FlowDocument();

            Paragraph paragraph = new Paragraph();
            foreach (string line in lines)
            {
                  MatchCollection matchReturn = Regex.Matches(line, pattern);
                if (matchReturn.Count != 0)
                {
                    int index = 0;
                    foreach (Match match in matchReturn)
                    {
                        paragraph.Inlines.Add(new Run(line.Substring(index, match.Index - index)));
                        index = match.Index + match.Length;

                        TextBlock textBlock = new TextBlock();
                        parameters[match.Value].TextControl.Add(textBlock);
                        textBlock.Text = parameters[match.Value].Value;
                        textBlock.Background = Brushes.Green;
                        paragraph.Inlines.Add(textBlock);
                    }
                    paragraph.Inlines.Add(new Run(line.Substring(index, line.Length - index)));
                }
                else
                {
                    paragraph.Inlines.Add(new Run(line));
                }
                paragraph.Inlines.Add(new LineBreak());
            }
            document.Blocks.Add(paragraph);

            return document;
        }
    }
}
