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
        public static bool StringMissing(string value)
        {
            return string.IsNullOrEmpty(value) ||  string.IsNullOrEmpty(value.Trim());
        }

        public static bool Email(string value)
        {
            // This regex pattern came from: http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase);
        }

        public static bool Number(string value)
        {
            foreach (char c in value)
            {
                if (!int.TryParse(c.ToString(), out int i))
                    return false;
            }
            return true;
        }

        public static bool MinLength(string value, int length)
        {
            return value.Length < length;
        }

        public static bool MaxLength(string value, int length)
        {
            return value.Length > length;
        }

        public static bool Double(string value)
        {
            return double.TryParse(value.ToString(), out double d);
        }

        public static bool Int(string value)
        { 
            return int.TryParse(value.ToString(), out int i);
        }

        public static string Format(string property, object model)
        {
            object value = model.GetType().GetProperty(property).GetValue(model);

            if (value == null)
                return null;

            switch (property)
            {
                case "Email":
                    return StringMissing(value.ToString()) || Email(value.ToString()) ? null : property + " ei ole kelvollinen.";
                case "ReferenceNumber":
                    {
                        if (!Number(value.ToString()))
                            return property + " ei ole kelvollinen";
                        if (MinLength(value.ToString(), Properties.Settings.Default.MinReferenceLength))
                            return property + " pitää olla vähintään " + Properties.Settings.Default.MinReferenceLength + " merkkiä pitkä.";
                        if (MaxLength(value.ToString(), Properties.Settings.Default.MaxReferenceLength))
                            return property + " ei saa olla pidempi kuin " + Properties.Settings.Default.MaxReferenceLength + " merkkiä.";
                        return null;
                    }
                case "Price":
                case "Count":
                    return Double(value.ToString()) ? null : property + " ei ole kelvollinen";
                case "DefaultInterest":
                    {
                        if (Double(value.ToString()))
                        {
                            if (double.Parse(value.ToString()) > Properties.Settings.Default.MaxInterest)
                                return property + " maksimiarvo on " + Properties.Settings.Default.MaxInterest;
                            return null;
                        }
                        return property + " ei ole kelvollinen.";
                    }
                    
                case "DefaultExpire":
                    return Int(value.ToString()) ? null : property + " ei ole kelvollinen";
                default:
                    return null;
            }
        }

        public static string Required(string property, object model)
        {
            object value = model.GetType().GetProperty(property).GetValue(model);

            if (value == null || StringMissing(value.ToString()))
                return property + " ei voi olla tyhjä.";
            return null;
        }

        public static string Unique(string property, DataModel datamodel)
        {
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
    }
}
