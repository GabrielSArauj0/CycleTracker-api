using Ciclo.Core.Authorization;
using Ciclo.Core.Enums;
using Ciclo.Application.Notifications;

namespace Ciclo.API.Controllers.V1.Administracao;

[ClaimsAuthorize("TipoUsuario", ETipoUsuario.Administrador)]
public abstract class MainController : BaseController
{
    protected MainController(INotificator notificator) : base(notificator)
    {
    }
}