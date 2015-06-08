using Medcenter.Service.Model;

namespace Medcenter.Service.Interface
{
    public class HelloService : ServiceStack.Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { UserName = "Hi, "+request.Name};
        }
    }
}
