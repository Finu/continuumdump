using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Collections;
using System.Windows.Documents;
using System.Windows.Media;

namespace ContinuumDump
{
    public class DataTemplate
    {
        private Dictionary<string, ParamValue> templateParameters = new Dictionary<string, ParamValue>();
        private ArrayList templateText = new ArrayList();
        private string pattern = "&&.*?&&";

        public ArrayList TemplateText
        {
            get { return templateText; }
        }

        public bool LoadTemplateFromFile(string FilePath)
        {
            templateParameters.Clear();
            templateText.Clear();
            if (File.Exists(FilePath))
            {
                using (StreamReader streamReader = new StreamReader(FilePath))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        DecodeSingleTemplateLine(line);
                    }
                    return true;
                }
            }
            return false;
        }

        public void DecodeSingleTemplateLine(string Line)
        {
            GetParamFromTemplateLine(Line);
            templateText.Add(Line);
        }

        private void GetParamFromTemplateLine(string Line)
        {
            foreach (Match match in Regex.Matches(Line, pattern))
            {
                if (!templateParameters.ContainsKey(match.Value))
                    templateParameters.Add(match.Value, new ParamValue(match.Value));
            }
        }

        public StringBuilder CreateOneLineToDump(IList<CSVParam> ParamToReplace)
        {
            StringBuilder stBuider = new StringBuilder();
            if (ParamToReplace != null)
            {
                foreach (string Line in templateText)
                {
                    string returnString = "";
                    MatchCollection matchReturn = Regex.Matches(Line, pattern);
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

        public void FillRichTextBox(RichTextBox RichBox)
        {
            if (RichBox!=null)
            {
                FlowDocument document = new FlowDocument();
                Paragraph paragraph = new Paragraph();
                foreach(string line in templateText)
                {
                    LineTemplateToParagraph(templateParameters, line, paragraph);
                    paragraph.Inlines.Add(new LineBreak());
                }
                document.Blocks.Add(paragraph);
                RichBox.Document = document;
            }
        }

        private void LineTemplateToParagraph(Dictionary<string, ParamValue> ParamToReplace, string Line, Paragraph Paragrap)
        {
            if (ParamToReplace != null)
            {
                MatchCollection matchReturn = Regex.Matches(Line, pattern);
                if (matchReturn.Count != 0)
                {
                    int index = 0;
                    foreach (Match match in matchReturn)
                    {
                        Paragrap.Inlines.Add(new Run(Line.Substring(index, match.Index - index)));
                        index = match.Index + match.Length;

                        TextBlock textBlock = new TextBlock();
                        ParamToReplace[match.Value].TextControl.Add(textBlock);
                        textBlock.Text = ParamToReplace[match.Value].Value;
                        textBlock.Background = Brushes.Green;
                        Paragrap.Inlines.Add(textBlock);
                    }
                    Paragrap.Inlines.Add(new Run( Line.Substring(index, Line.Length - index)));
                }
                else
                {
                    Paragrap.Inlines.Add(new Run(Line));
                }
            }
        }

        public StringBuilder GetTexFromFlowDocument(FlowDocument Document)
        {
            StringBuilder stBuider = new StringBuilder();
            if (Document != null)
            {
                string returnString = "";
                foreach (Block block in Document.Blocks)
                {
                    Paragraph paragraph = block as Paragraph;
                    if (paragraph != null)
                    {
                        foreach (Inline inline in paragraph.Inlines)
                        {
                            if (inline is Run)
                            {
                                Run run = inline as Run;
                                returnString += run.Text;
                            }
                            else if (inline is InlineUIContainer)
                            {
                                InlineUIContainer container = inline as InlineUIContainer;
                                TextBlock text = container.Child as TextBlock;
                                returnString += text.Text;
                            }
                            else if (inline is LineBreak)
                            {
                                stBuider.AppendLine(returnString);
                                returnString = "";
                            }
                        }
                    }
                }
                stBuider.Remove(stBuider.Length - 2, 2);
            }
            return stBuider;
        }
    }
}
