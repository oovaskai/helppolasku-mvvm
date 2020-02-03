using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;
using HelppoLasku.DataAccess;

namespace HelppoLasku.Validation
{
    public static class Validation
    {
        #region Validation Methods

        public static string Required(string property, object model)
        {
            PropertyInfo info = model.GetType().GetProperty(property);
            if (info == null)
                return property + " haloo olli??"; //debugging: property not found

            object value = info.GetValue(model);

            if (value == null || StringMissing(value.ToString()))
                return property + " ei voi olla tyhjä.";
            return null;
        }

        public static string Unique(string property, DataModel datamodel, bool required)
        {
            string error = Required(property, datamodel);
            if (error != null)
                return required ? error : null;

            foreach (DataModel model in Resources.GetRepository(datamodel).Models)
            {
                if (datamodel.IsNew || datamodel.ID != model.ID)
                {
                    object thisValue = datamodel.GetType().GetProperty(property).GetValue(datamodel);
                    object targetValue = model.GetType().GetProperty(property).GetValue(model);

                    if (!(thisValue != null && targetValue == null) && !(thisValue == null && targetValue != null) && !(thisValue == null && targetValue == null))
                    {
                        if (thisValue.ToString().ToLower() == targetValue.ToString().ToLower())
                            return property + " on jo käytössä";
                    }
                }
            }
            return null;
        }

        public static string Format(string property, object model, bool required)
        {
            string error = Required(property, model);
            if (error != null)
                return required ? error : null;

            string value = model.GetType().GetProperty(property).GetValue(model).ToString();

            switch (property)
            {
                case "Email":
                    return StringMissing(value) || Email(value) ? null : property + " ei ole kelvollinen.";
                case "Phone":
                    return Phone(property, value);
                case "ReferenceBase":
                    {
                        if (!OnlyNumbers(value))
                            return property + " ei ole kelvollinen";
                        return Length(property, value, Properties.Settings.Default.MinReferenceLength, Properties.Settings.Default.MaxReferenceLength);
                    }
                case "Price":
                case "Count":
                    return DoubleRange(property, value, 0, null);
                case "CompanyInterest":
                case "Interest":
                    return DoubleRange(property, value, 0, Properties.Settings.Default.MaxInterest);
                case "CompanyAnnotation":
                case "PersonAnnotation":
                case "AnnotationTime":
                case "ExpireDays":
                    return IntRange(property, value, 0, null);
                case "CompanyExpire":
                    return IntRange(property, value, Properties.Settings.Default.MinCompanyExpire, null);
                case "PersonExpire":
                    return IntRange(property, value, Properties.Settings.Default.MinPersonExpire, null);
                default:
                    return null;
            }
        }

        #endregion

        #region Helper methods

        public static bool StringMissing(string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim());
        }

        public static bool Email(string value)
        {
            // This regex pattern came from: http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase);
        }

        public static string Phone(string property, string value)
        {
            string phone = value;

            if (phone.IndexOf('+') == 0)
                phone = phone.Remove(0,1);
            if (phone.IndexOf('-') == 3 )
                phone = phone.Remove(3,1);

            if (OnlyNumbers(phone) && !phone.Contains('+') && !phone.Contains('-'))
            {
                if (value.Contains('+'))
                    return Length(property, value, 13, 13);
                if (value.Contains('-'))
                    return Length(property, value, 11, 11);
                else
                    return Length(property, value, 10, 10);
            }
            return property + " ei ole kelvollinen.";
        }

        public static bool OnlyNumbers(string value)
        {
            foreach (char c in value)
            {
                if (!int.TryParse(c.ToString(), out int i))
                    return false;
            }
            return true;
        }

        public static string Length(string property, string value, int? min, int? max)
        {
            string error = null;
            if (min != null && min > value.Length)
                error = property + " pitää olla vähintään " + min + " merkkiä pitkä.";

            if (max != null && max < value.Length)
                error = property + " maksimipituus on " + max + " merkkiä.";

            return error;
        }

        public static string Double(string property, string value, out double d)
        {
            return double.TryParse(value, out d) ? null : property + " ei ole kelvollinen.";
        }

        public static string Int(string property, string value, out int i)
        {
            return int.TryParse(value, out i) ? null : property + " ei ole kelvollinen.";
        }

        public static string IntRange(string property, string value, int? min, int? max)
        {
            string error = Int(property, value, out int i);
            
            if (min != null && min > i)
                error = property + " minimiarvo on " + min;

            if (max != null && max < i)
                error = property + " maksimiarvo on " + max;

            return error;
        }

        public static string DoubleRange(string property, string value, double? min, double? max)
        {
            string error = Double(property, value, out double d);

            if (min != null && min > d)
                error = property + " minimiarvo on " + min + ".";

            if (max != null && max < d)
                error = property + " maksimiarvo on " + max + ".";

            return error;
        }

        #endregion
    }
}
