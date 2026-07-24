namespace ExamForge.Application.Common.Abstractions.Requests;

public interface ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<TResponse> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken = default);
}
