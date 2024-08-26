using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Wolverine.Http;

namespace CritterStack.Todos;

public static  class TodoGetEndpoints
{
    [WolverineGet("/api/todos/{id:guid}")]
    public static async Task<Results<Ok<Todo>, NotFound>> GetTodo(Guid id, IDocumentSession session,
        CancellationToken token)
    {
        var todo = await session.LoadAsync<Todo>(id, token);
        if (todo is null)
        {
            return TypedResults.NotFound();
        }
        else
        {
            return TypedResults.Ok(todo);
        }
    }

    [WolverineGet("/api/todos")]
    public static async Task<Ok<IReadOnlyList<Todo>>> GetAllTodos(IDocumentSession session)
    {
        var response = await session.Query<Todo>().ToListAsync();
        return TypedResults.Ok(response);
    }
}