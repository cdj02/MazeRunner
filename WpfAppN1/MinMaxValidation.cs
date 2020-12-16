using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace WpfAppN1
{
    public class MinMaxValidationRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int number;

            // Is a number?
            if (!int.TryParse((string)value, out number))
            {
                return new ValidationResult(false, "Not a number.");
            }

            // Is in range?
            if ((number < Min) || (number > Max))
            {
                var msg = $"Number must be between {Min} and {Max}.";
                return new ValidationResult(false, msg);
            }

            // Number is valid
            return new ValidationResult(true, null);
        }

    }
}
