using System;
using System.Collections.Generic;
using System.Text;

namespace NhsDemoApp.Models
{
    public class ExcelModel
    {
        public List<string> Headers { get; set; } = new List<string>();
        public List<List<string>> Values { get; set; } = new List<List<string>>();
    }
}
