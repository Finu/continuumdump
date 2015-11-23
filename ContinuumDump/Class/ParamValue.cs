using System.Collections.Generic;
using System.Windows.Controls;

namespace ContinuumDump
{
    public class ParamValue
    {
        private List<TextBlock> textControl = new List<TextBlock>();

        public string Value { get; set; }
        public ParamState State { get; set; }

        public List<TextBlock> TextControl
        {
            get { return textControl; }
        }

        public ParamValue(string Value)
        {
            this.Value = Value;
            this.State = ParamState.ps_UnPaired;
        }
    }
}
