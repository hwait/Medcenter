using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace Medcenter.Service.Model
{
    [Route("/hello/{Name}")]
    //[DataContract]
    public class Hello : IReturn<HelloResponse>
    {
      //  [DataMember]
        public string Name { get; set; }
    }
    public class HelloResponse : IHasResponseStatus
    {
        public string Result { get; set; }
        public string UserName { get; set; }
        public ResponseStatus ResponseStatus { get; set; } 
    }
}
