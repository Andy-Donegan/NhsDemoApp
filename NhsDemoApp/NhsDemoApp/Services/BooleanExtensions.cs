using System;
using System.Collections.Generic;
using System.Text;

namespace NhsDemoApp.Services
{
    public static class BooleanExtensions
    {
        public static string ToYesNoString(this bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}
