using Ciclo.Application.Notifications;
using Ciclo.Core.Authorization;
using Ciclo.Core.Enums;

namespace Ciclo.API.Controllers.V1.Usuario;


public abstract class MainController : BaseController
{
    protected MainController(INotificator notificator) : base(notificator)
    {
    }
}