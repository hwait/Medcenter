using Medcenter.Service.Model.Types;

namespace Medcenter.Desktop.Infrastructure
{
    public interface IUserRepository
    {
        User CurrentUser { get; set; }
    }
}
