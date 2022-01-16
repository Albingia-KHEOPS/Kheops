using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models
{
    public class LabeledValue
    {
        protected string label;

        public LabeledValue() { }

        public LabeledValue(string code, string label)
        {
            Code = code;
            Label = label;
        }

        public long Id { get; set; }

        public string Code { get; set; }

        public string Label { get; set; }

        public string Code_Label
        {
            get
            {
                var array = new List<string>();
                if (!string.IsNullOrWhiteSpace(Code)) array.Add(Code);
                if (!string.IsNullOrWhiteSpace(Label)) array.Add(Label);
                if (array.Count < 2) return array.FirstOrDefault() ?? string.Empty;

                return string.Join(" - ", array);
            }
        }

        public string Parent { get; set; }

        public static LabeledValue Create(string code, string label)
        {
            return new LabeledValue(code, label);
        }
    }
}