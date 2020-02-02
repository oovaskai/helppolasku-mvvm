using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelppoLasku.Models
{
    public class Company : Contact
    {
        public Company() : base() { }

        public Company(DataAccess.DataModel source) : base(source) { }

        public string Logo { get; set; }

        public string BIC { get; set; }

        public string IBAN { get; set; }

        public int DefaultExpire { get; set; } // HETI jos myöhässä || > 7 yrityksille, >= 14

        public double DefaultInterest { get; set; } // max 11,5 %  yrityksille, aina kuluttajille

        public string ReferenceNumber { get; set; } // min 4 merkkiä, max 19 merkkiä + tarkiste && generoi asnro + laskunro || juokseva

        public int InvoiceID { get; set; } // juokseva

        public double DefaultTax { get; set; }

        internal override string[] CopyProperties
        {
            get
            {
                string[] props = base.CopyProperties.Concat(new string[]
                {
                    "Logo",
                    "BIC",
                    "IBAN",
                    "DefaultExpire",
                    "DefaultInterest",
                    "ReferenceNumber",
                    "InvoiceID",
                    "DefaultTax"
                }).ToArray();

                return props;
            }
        }

        #region Methods

        public string GetNewReference()
        {
            int firstNonZero = -1;

            for (int i = 0; i < ReferenceNumber.Length; i++)
            {
                if (ReferenceNumber[i] != '0')
                    if (firstNonZero == -1)
                        firstNonZero = i;
            }

            string newRef = "";

            if (firstNonZero > -1)
            {
                newRef = ReferenceNumber.Substring(firstNonZero);

                if (newRef.Length > 0)
                {
                    int intRef = int.Parse(newRef);
                    intRef++;
                    newRef = intRef.ToString();
                }
                else
                    newRef = "1";
            }

            if (newRef.Length > ReferenceNumber.Length)
                return newRef + ReferenceCheck(newRef).ToString();
            
            if (newRef.Length < 1)
                newRef = "1";

            string newReference = ReferenceNumber.Remove(ReferenceNumber.Length - newRef.Length) + newRef;
            return newReference + ReferenceCheck(newReference).ToString();
        }

        public int ReferenceCheck(string reference)
        {
            int sum = 0;

            string revref = Reverse(reference);

            int[] multipliers = new int[] { 7, 3, 1 };

            int i = 0;

            foreach (char c in revref)
            {
                if (int.TryParse(c.ToString(), out int value))
                    sum += value * multipliers[i];

                i++;
                if (i == 3)
                    i = 0;
            }

            int check = 0;

            if (sum % 10 != 0)
                check = 10 - sum % 10;

            if (check == 10)
                check = 0;

            return check;
        }

        string Reverse(string value)
        {
            char[] charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        #endregion
    }
}
