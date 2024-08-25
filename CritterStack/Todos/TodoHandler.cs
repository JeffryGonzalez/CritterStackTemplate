using Marten;

namespace CritterStack.Todos;

public static class TodoHandler
{
    public static void Handle(TodoCreated msg, ILogger logger, IDocumentSession session)
    {
        // Nada.
        logger.LogInformation("Got a create todo {id}", msg.Id);
         session.Events.StartStream(msg);
         session.Events.Append(msg.Id, msg);
    }
}
public record CreateTheTodo(Guid Id);