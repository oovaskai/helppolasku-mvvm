using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using HelppoLasku.Models;
using HelppoLasku.Test;

namespace HelppoLasku.DataAccess
{
    public static class Resources
    {
        static List<DataRepository> repositories;

        static List<DataRepository> Repositories
        {
            get
            {
                if (repositories == null)
                    repositories = new List<DataRepository>();
                return repositories;
            }
        }

        public static void Initialize()
        {
            TestData.Init();

            Clear();
            
            AddRepository<Company>(new TestHandler(TestData.Companies));
            AddRepository<ProductGroup>(new TestHandler(TestData.Groups));
            AddRepository<Product>(new TestHandler(TestData.Products));
            AddRepository<Customer>(new TestHandler(TestData.Customers));
            AddRepository<Invoice>(new TestHandler(TestData.Invoices));
        }

        public static void AddRepository<T>(IDataHandler handler)
        {
            if (GetRepository(typeof(T)) == null)
                Repositories.Add(new DataRepository(typeof(T), handler));
        }

        public static DataRepository GetRepository(DataModel model)
        {
            return GetRepository(model.GetType());
        }

        public static DataRepository GetRepository(Type type)
        {
            foreach (DataRepository repository in Repositories)
            {
                if (repository.DataType == type)
                    return repository;
            }
            return null;
        }

        public static ObservableCollection<DataModel> GetModels<T>()
        {
            DataRepository repository = GetRepository(typeof(T));
            if (repository != null)
                return repository.Models;
            return null;
        }

        public static void Clear()
        {
            repositories = null;
        }
    }
}
