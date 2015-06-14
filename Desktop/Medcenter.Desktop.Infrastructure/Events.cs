using Medcenter.Service.Model.Messaging;
using Medcenter.Service.Model.Types;
using Microsoft.Practices.Prism.PubSubEvents;

namespace Medcenter.Desktop.Infrastructure
{
    public class UserLoginEvent : PubSubEvent<User>
    {
        
    }
    public class UserInfoEvent : PubSubEvent<User>
    {

    }
    public class OperationResultEvent : PubSubEvent<ResultMessage>
    {

    }
}
