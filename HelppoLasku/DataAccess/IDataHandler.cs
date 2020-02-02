using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.Models;

namespace HelppoLasku.DataAccess
{
    public interface IDataHandler
    {
        List<DataModel> GetAll();

        List<DataModel> GetIf(string where, string isValue);

        DataModel GetByID(string id);

        DataModel Create(DataModel model);

        bool Update(DataModel model);

        bool Delete(DataModel model); 
    }
}
