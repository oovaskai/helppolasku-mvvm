using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using HelppoLasku.Models;

namespace HelppoLasku.DataAccess
{
    public class DataRepository
    {
        IDataHandler Handler;

        public DataRepository(Type dataType, IDataHandler handler)
        {
            if (!dataType.IsSubclassOf(typeof(DataModel)))
                throw new Exception("DataRepository Type must be a subclass of DataAccess.DataModel");

            DataType = dataType;
            Handler = handler;
        }

        public Type DataType { get; private set; }

        ObservableCollection<DataModel> models;

        public ObservableCollection<DataModel> Models
        {
            get
            {
                if (models == null)
                    models = new ObservableCollection<DataModel>(Handler.GetAll()); 
                  
                return models;
            }
        }

        public virtual void Save(DataModel model)
        {
            DataModel modelInRepo = FindByID(model.ID);
            if (modelInRepo == null)
            {
                Models.Add(Handler.Create(model));
            }
            else
            {
                if (Handler.Update(model))
                {
                    model.CopyTo(modelInRepo, out string[] updatedProperties);
                    modelInRepo.ModelChanged?.Invoke(modelInRepo, ModelChangedEventArgs.Update(updatedProperties));
                }      
            }
        }

        public virtual void Delete(DataModel model)
        {
            if (Handler.Delete(model))
            {
                model.ModelChanged?.Invoke(model, ModelChangedEventArgs.Delete);
                Models.Remove(model);
            }     
        }

        public virtual DataModel FindByID(string id)
        {
            foreach (DataModel model in Models)
                if (model.ID == id)
                    return model;
            return null;
        }
    }
}
