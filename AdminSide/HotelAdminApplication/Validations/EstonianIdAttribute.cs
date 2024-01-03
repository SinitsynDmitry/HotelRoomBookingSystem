/******************************************************************************
 *
 * File: EstonianIdAttribute.cs
 *
 * Description: EstonianIdAttribute.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using System.ComponentModel.DataAnnotations;

namespace HotelAdminApplication.Validations
{
    /// <summary>
    /// The estonian id validation attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class EstonianIdAttribute : ValidationAttribute
    {
        /// <summary>
        /// Are the valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A bool.</returns>
        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                return true;
            }
            var id = value.ToString();

            if (id.Length != 11 || !IsNumeric(id))
            {
                ErrorMessage = "Estonian ID must be 11 digits and contain only numeric characters.";
                return false;
            }

            if (!IsValidEstonianId(id))
            {
                ErrorMessage = "Invalid Estonian ID.";
                return false;
            }
            return true;
        }


        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    if (value == null)
        //    {
        //        // You may choose to handle null values differently based on your requirements
        //        return ValidationResult.Success;
        //    }

        //    var id = value.ToString();

        //    if (id.Length != 11 || !IsNumeric(id))
        //    {
        //        return new ValidationResult("Estonian ID must be 11 digits and contain only numeric characters.");
        //    }

        //    if (!IsValidEstonianId(id))
        //    {
        //        return new ValidationResult("Invalid Estonian ID.");
        //    }

        //    return ValidationResult.Success;
        //}

        /// <summary>
        /// Are the numeric.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A bool.</returns>
        private bool IsNumeric(string input)
        {
            return long.TryParse(input, out _);
        }

        /// <summary>
        /// Are the valid estonian id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A bool.</returns>
        private bool IsValidEstonianId(string id)
        {
            // Check the century and extract birthdate components
            int century = int.Parse(id.Substring(0, 1));
            int year = int.Parse(id.Substring(1, 2));
            int month = int.Parse(id.Substring(3, 2));
            int day = int.Parse(id.Substring(5, 2));

            // Check the century and adjust the year
            int baseYear = GetBaseYear(century);
            int fullYear = baseYear + year;

            // Attempt to create a DateTime object, and validate the birthdate
            if (DateTime.TryParse($"{fullYear}-{month}-{day}", out DateTime birthDate))
            {
                var multiplier_1 = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 };
                var multiplier_2 = new int[10] { 3, 4, 5, 6, 7, 8, 9, 1, 2, 3 };

                var control = (int)char.GetNumericValue(id[10]);

                var mod = 0;
                var total = 0;
                /* Do first run. */
                for (int i = 0; i < 10; i++)
                {
                    total += (int)char.GetNumericValue(id[i]) * multiplier_1[i];
                }
                mod = total % 11;

                /* If modulus is ten we need second run. */
                total = 0;
                if (10 == mod)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        total += (int)char.GetNumericValue(id[i]) * multiplier_2[i];
                    }
                    mod = total % 11;

                    /* If modulus is still ten revert to 0. */
                    if (10 == mod)
                    {
                        mod = 0;
                    }
                }
                return control == mod;
            }

            return false;
        }

        /// <summary>
        /// Gets the base year.
        /// </summary>
        /// <param name="century">The century.</param>
        /// <returns>An int.</returns>
        private int GetBaseYear(int century)
        {
            switch (century)
            {
                case 1:
                case 2: return 1800;
                case 3:
                case 4: return 1900;
                case 5:
                case 6: return 2000;
                case 7:
                case 8: return DateTime.Now.Year < 2100 ? 2100 : 0;
                default: return 0; // Invalid century
            }
        }
    }

}
