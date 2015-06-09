using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Service.Model.Messaging
{
    [DataContract]
    public class ResultMessage
    {
        // 0 - success, 1 - warning, 2 - error
        private string _message;
        private DateTime _dt;
        private int _errorType;
        private string _source;
        [DataMember]
        public DateTime Dt
        {
            get { return _dt; }
            set { _dt = value; }
        }
        [DataMember]
        public int ErrorType
        {
            get { return _errorType; }
            set { _errorType = value; }
        }
        [DataMember]
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }
        [DataMember]

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public ResultMessage(int errorType, string source, string message, DateTime dt=default(DateTime))
        {
            _errorType = errorType;
            _source = source;
            _message = message;
            _dt = dt;
        }
    }
}
