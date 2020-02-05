using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.DataAccess;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class CompanyMenuItemViewModel : DataViewModel
    {
        public CompanyMenuItemViewModel(Company company) : base(company)
        {
        }

        public CompanyMenuItemViewModel(Company company, CompanyMenuViewModel menu) : base(company)
        {
            CompanyMenu = menu;
        }

        #region Properties

        public new Company Model
        {
            get => base.Model as Company;
            set => base.Model = value;
        }

        public string Name => Model.Name;

        CompanyMenuViewModel companyMenu;

        public CompanyMenuViewModel CompanyMenu
        {
            get => companyMenu;
            set
            {
                if (companyMenu != value)
                {
                    if (value != null)
                        value.SelectionChanged += OnCompanyMenuSelectionChange;

                    companyMenu = value;
                }
            }
        }

        #endregion

        #region IsSelected

        public bool IsSelected => CompanyMenu.SelectedItem == this;

        void OnCompanyMenuSelectionChange(object sender, SelectionChangedEventArgs e) 
            => RaisePropertyChanged("IsSelected");

        public CommandViewModel Select => new CommandViewModel("Vaihda yritystä", OnSelect, CanSelect);

        void OnSelect() 
            => CompanyMenu.SelectedItem = this;

        bool CanSelect()
            => !IsSelected;

        #endregion

        #region Base Overrides

        public override void OnModelChanged(object sender, ModelChangedEventArgs e)
        {
            if (e.Properties != null && e.Properties.Length > 0)
            {
                if (e.Properties.Contains("Name"))
                    CompanyMenu.DisplayName = Name;
            }
            base.OnModelChanged(sender, e);
        }

        protected override void OnDispose()
        {
            companyMenu.SelectionChanged -= OnCompanyMenuSelectionChange;
            base.OnDispose();
        }

        #endregion
    }
}
