using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.DataAccess;
using HelppoLasku.ViewModels;

namespace HelppoLasku.Validation
{
    public abstract class DataValidator
    {   
        protected DataValidator(DataViewModel viewmodel)
        {
            ViewModel = viewmodel;
            Model = viewmodel.Model;
        }

        string[] validationProperties;

        string[] ValidationProperties
        {
            get
            {
                if (validationProperties == null)
                    validationProperties = Properties;

                return validationProperties;
            }
        }

        public DataModel Model { get; private set; }

        public DataViewModel ViewModel { get; private set; }

        protected abstract string[] Properties { get; }

        public abstract string Validate(string property);

        public string GetError()
        {
            foreach (string property in ValidationProperties)
            {
                string result = Validate(property);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
