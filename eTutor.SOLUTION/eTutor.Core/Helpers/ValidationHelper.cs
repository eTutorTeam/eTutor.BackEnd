using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using FluentValidation.Results;

namespace eTutor.Core.Helpers
{
    public static class ValidationHelper
    {
        public static string JSONFormatErrors(this ValidationResult validator)
        {
            string str = "";

            if (!validator.IsValid)
            {
                for (var index = 0; index < validator.Errors.Count; index++)
                {
                    var failure = validator.Errors[index];
                    str += $"{failure.PropertyName}: {failure.ErrorMessage}";

                    if (index + 1 < validator.Errors.Count) str += ",   ";
                }

            }

            return str;
        }
    }
}
