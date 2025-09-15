using discipline.libs.cqrs.abstractions.Commands;
using discipline.libs.cqrs.abstractions.Queries;

namespace discipline.libs.cqrs.Abstractions;

public interface ICqrsDispatcher
{
    Task HandleAsync<TCommand>(TCommand command, CancellationToken cancellationToken) 
        where TCommand : class, ICommand;
    
    Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
}