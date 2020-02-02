using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.Models;
using HelppoLasku.DataAccess;

namespace HelppoLasku.Test
{
    public class TestHandler : IDataHandler
    {
        List<DataModel> source;

        public TestHandler(List<DataModel> source)
        {
            this.source = source;
        }

        public DataModel Create(DataModel model)
        {
            model.ID = TestData.IdGen.Next();
            return model;
        }

        public bool Delete(DataModel model)
        {
            return true;
        }

        public List<DataModel> GetAll()
        {
            return source;
        }

        public DataModel GetByID(string id)
        {
            return source.Find((model) => model.ID == id);
        }

        public List<DataModel> GetIf(string where, string isValue)
        {
            throw new NotImplementedException();
        }

        public bool Update(DataModel model)
        {
            return true;
        }
    }
}
