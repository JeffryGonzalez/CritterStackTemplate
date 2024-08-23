using Marten;
using Wolverine.Http;

namespace CritterStack.Todos;

public record TodoCreationResponse(Guid Id) : CreationResponse("/api/todos/" + Id);

public class Todo
{
    public Guid Id { get; set; }
    public string Description { get; set; } = "";
    public bool IsComplete { get; set; } = false;
}

public record CreateTodo
{
    public string Description { get; set;} = string.Empty;
}

public record TodoCreated(Guid Id);
public static class TodoCreateEndpoint
{
    [WolverinePost("/api/todos")]
    public static (TodoCreationResponse, TodoCreated) Post(CreateTodo command, IDocumentSession session)
    {
        var todo = new Todo()
        {
            Description = command.Description,
            Id = Guid.NewGuid(),
        };
        session.Store(todo);
        return (new TodoCreationResponse(todo.Id), new TodoCreated(todo.Id));
    }
    
}