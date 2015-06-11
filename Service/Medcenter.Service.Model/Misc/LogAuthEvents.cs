using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Web;

namespace Medcenter.Service.Model.Misc
{
    public class LogAuthEvents : AuthEvents
    {
        
        public override void OnLogout(IRequest httpReq, IAuthSession session, IServiceBase authService)
        {
            IDbConnectionFactory dbf = HostContext.TryResolve<IDbConnectionFactory>();
            IDbConnection db = dbf.OpenDbConnection();
            var id = db.Single<int>("EXEC sp_UserSessions_logout @SessionId", new
            {
                SessionId = session.Id
            });
        }
    }
}
