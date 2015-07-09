using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Messaging;

namespace Medcenter.Desktop.Modules.StatusbarModule.SampleData
{
    public class DesignData
    {
        public ResultMessage AggregateMessage
        {
            get { return new ResultMessage(2, "Операция сохранение", "Операция завершена с 3 ошибками."); }
        }

        public List<ResultMessage> StatusMessages
        {
            get
            {
                return new List<ResultMessage>
                {
                    new ResultMessage(0, "Молуль А", "Операция проведена успешно.",DateTime.Now),
                    new ResultMessage(1, "Молуль Б", "Обратите внимание!",DateTime.Now),
                    new ResultMessage(2, "Молуль В", "Ошибка!",DateTime.Now)
                };
            }
        }
    }
}
