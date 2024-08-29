// Provides us with APIs that we can use for our host
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Allow us to make route handlers
var app = builder.Build();

// Start server
app.Run();

// Create a list 
var todos = new List<Todo>();

// [GET]: Return list of todos
app.MapGet("/todos", () => todos);

// [GET]: Find a todo item
app.MapGet("/todos/{id}", Results<Ok<Todo>, NotFound> (int id) =>
{
    var targetTodo = todos.SingleOrDefault(t => id == t.id);
    return targetTodo is null
        ? TypedResults.NotFound()
        : TypedResults.Ok(targetTodo);
});

// [POST]: Create a todo
app.MapPost("/todos", (Todo task) =>
{
    todos.Add(task);
    return TypedResults.Created("/todos/{id}", task);
});

// [DELETE]: Delete an item
app.MapDelete("/todos/{id}", (int id) => 
{
    todos.RemoveAll(t => id == t.id);
    return TypedResults.NoContent();
});

// [ Record]: Simple immutable data model
public record Todo(int id, string Name, DateTime DueDate, bool IsCompleted);

