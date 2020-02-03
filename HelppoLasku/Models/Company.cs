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

        public int InvoiceID { get; set; }

        public string ReferenceBase { get; set; }

        public string Reference => ReferenceBase + ReferenceCheck(ReferenceBase);

        public int CompanyExpire { get; set; }

        public double CompanyInterest { get; set; }

        public int CompanyAnnotation { get; set; }

        public int PersonExpire { get; set; }

        public double PersonInterest { get; set; }

        public int PersonAnnotation { get; set; }
        
        public double Tax { get; set; }

        internal override string[] CopyProperties
        {
            get
            {
                string[] props = base.CopyProperties.Concat(new string[]
                {
                    "Logo",
                    "BIC",
                    "IBAN",
                    "CompanyExpire",
                    "CompanyInterest",
                    "CompanyAnnotation",
                    "PersonExpire",
                    "PersonInterest",
                    "PersonAnnotation",
                    "ReferenceBase",
                    "InvoiceID",
                    "Tax",
                }).ToArray();

                return props;
            }
        }

        #region Methods

        public void NewInvoice()
        {
            int firstNonZero = -1;

            for (int i = 0; i < ReferenceBase.Length; i++)
            {
                if (ReferenceBase[i] != '0')
                    if (firstNonZero == -1)
                        firstNonZero = i;
            }

            string newRef = "";

            if (firstNonZero > -1)
            {
                newRef = ReferenceBase.Substring(firstNonZero);

                if (newRef.Length > 0)
                {
                    int intRef = int.Parse(newRef);
                    intRef++;
                    newRef = intRef.ToString();
                }
            }

            if (newRef.Length > ReferenceBase.Length)
                ReferenceBase = newRef;
            
            if (newRef.Length < 1)
                newRef = "1";

            ReferenceBase = ReferenceBase.Remove(ReferenceBase.Length - newRef.Length) + newRef;
            InvoiceID++;
            Save();
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
