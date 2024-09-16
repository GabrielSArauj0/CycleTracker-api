using Ciclo.Application.Notifications;

namespace Ciclo.API.Controllers.V1;

public abstract class MainController : BaseController
{
    protected MainController(INotificator notificator) : base(notificator)
    {
    }
}