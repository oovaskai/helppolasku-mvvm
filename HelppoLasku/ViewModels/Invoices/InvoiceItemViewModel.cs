using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.DataAccess;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class InvoiceItemViewModel : DataViewModel
    {
        public InvoiceItemViewModel(InvoiceItem item) : base(item)
        {          
        }

        public new InvoiceItem Model
        {
            get => base.Model as InvoiceItem;
            set => base.Model = value;
        }

        #region Properties

        public InvoiceTitleViewModel Title { get; set; }

        public int Index
        {
            get => Model.Index;
            set
            {
                if (Model.Index != value)
                {
                    Model.Index = value;
                    RaisePropertyChanged("Index");
                }
            }
        }

        public virtual string Content
        {
            get => Model.Content;
            set
            {
                if (Model.Content != value)
                {
                    Model.Content = value;
                    RaisePropertyChanged("Content");
                }
            }
        }

        public virtual string Count
        {
            get => Model.Count.ToString();
            set { }
        }

        public string Unit
        {
            get => Model.Unit;
            set
            {
                if (Model.Unit != value)
                {
                    Model.Unit = value;
                    RaisePropertyChanged("Unit");
                }
            }
        }

        public double TaxlessPrice
        {
            get => Model.Tax < 0 ? Model.Price - (Model.Price * Tax / (Tax + 100)) : Model.Price;
            set
            {
                if (Model.Price != value)
                {
                    Model.Price = value;
                    RaisePriceChanged();
                }
            }
        }

        public virtual double Tax
        {
            get => Model.Tax < 0 ? Model.Tax * -1 : Model.Tax;
            set
            {
                if (Model.Tax != value)
                {
                    Model.Tax = value;
                    RaisePriceChanged();
                }
            }
        }

        public double Taxed => Total * Tax / (100 + Tax);

        public double Taxless => Total - Taxed;

        public virtual double Total => TaxlessPrice * (1 + Tax / 100) * Model.Count;

        public string Info
        {
            get => Model.Info;
            set
            {
                if (Model.Info != value)
                {
                    Model.Info = value;
                    RaisePropertyChanged("Info");
                }
            }
        }

        #endregion

        #region Methods

        void RaisePriceChanged()
        {
            RaisePropertyChanged("TaxlessPrice");
            RaisePropertyChanged("Tax");
            RaisePropertyChanged("Taxless");
            RaisePropertyChanged("Taxed");
            RaisePropertyChanged("Total");
        }

        #endregion
    }
}
