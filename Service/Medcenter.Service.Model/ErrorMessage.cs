using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Service.Model
{
    public class ErrorMessage
    {
        private string _message;
        private string _source;

        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public ErrorMessage(string source, string message)
        {
            _source = source;
            _message = message;
        }
    }
}
