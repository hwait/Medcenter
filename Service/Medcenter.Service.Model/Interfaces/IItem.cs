using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;

namespace Medcenter.Service.Model.Interfaces
{
    public interface IItem
    {
        int Id { get; set; }
        string Name { get; set; }
        string ShortName { get; set; }
        List<int> InspectionGroupIds { get; set; }
        List<ResultMessage> Validate();
    }
}
