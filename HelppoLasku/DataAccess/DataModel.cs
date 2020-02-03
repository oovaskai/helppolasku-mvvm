using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace HelppoLasku.DataAccess
{
    public abstract class DataModel
    {
        protected DataModel() { }

        protected DataModel(DataModel source) 
        {
            ID = source.ID;
            source.CopyTo(this);
        }

        public string ID { get; set; }

        public bool IsNew => string.IsNullOrEmpty(ID);

        internal abstract string[] CopyProperties { get; }

        public virtual void CopyTo(DataModel target)
        {
            try
            {
                foreach (string property in CopyProperties)
                    target.GetType().GetProperty(property).SetValue(target, GetType().GetProperty(property).GetValue(this)); 
            }
            catch (Exception e)
            {
                Views.MainWindow.Message(e.Message, "Kopiointivirhe", System.Windows.MessageBoxImage.Error);
            }                
        }

        public virtual void CopyTo(DataModel target, out string[] copiedProperties)
        {
            List<string> changedProps = new List<string>();

            try
            {
                foreach (string property in CopyProperties)
                {
                    object thisValue = GetType().GetProperty(property).GetValue(this);
                    object targetValue = target.GetType().GetProperty(property).GetValue(target);

                    if (!(thisValue == null && targetValue == null) && ((thisValue == null && targetValue != null) ||
                        (thisValue != null && targetValue == null) || thisValue.ToString() != targetValue.ToString() || !thisValue.Equals(targetValue)))
                    {
                        target.GetType().GetProperty(property).SetValue(target, GetType().GetProperty(property).GetValue(this));
                        changedProps.Add(property);
                    }
                }   
            }
            catch (Exception e)
            {
                Views.MainWindow.Message(e.Message, "Kopiointivirhe", System.Windows.MessageBoxImage.Error);
            }

            copiedProperties = changedProps.ToArray();
        }

        public EventHandler<ModelChangedEventArgs> ModelChanged;

        public virtual void Save() => Resources.GetRepository(this).Save(this);

        public virtual void Delete() => Resources.GetRepository(this).Delete(this);
    }

    public class ModelChangedEventArgs : EventArgs
    {
        public enum EventType { Update, Delete, Empty }

        public static ModelChangedEventArgs Update(string[] properties) => new ModelChangedEventArgs { Type = EventType.Update, Properties = properties };

        public static ModelChangedEventArgs Delete => new ModelChangedEventArgs { Type = EventType.Delete };

        public new static ModelChangedEventArgs Empty => new ModelChangedEventArgs { Type = EventType.Empty };

        public EventType Type { get; private set; }

        public string[] Properties { get; private set; }
    }
}
