﻿namespace ContinuumDump
{
    public class CSVParam
    {
        public string Parameter { get; set; }
        public string Value { get; set; }

        public CSVParam(string Parameter, string Value)
        {
            this.Parameter = Parameter;
            this.Value = Value;
        }
    }
}
