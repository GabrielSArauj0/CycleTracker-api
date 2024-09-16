using FluentValidation.Results;

namespace Ciclo.Application.Notifications;

public interface INotificator
{
    void Handle(string mensagem);
    void Handle(List<ValidationFailure> failures);
    void HandleNotFoundResource();
    IEnumerable<string> GetNotifications();
    bool HasNotification { get; }
    bool IsNotFoundResource { get; }
}