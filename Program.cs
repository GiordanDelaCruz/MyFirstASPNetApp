// Provides us with APIs that we can use for our host
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

// Allow us to make route handlers
var app = builder.Build();

// Redirect the user to /todos route when they want to took for a task
app.UseRewriter(new RewriteOptions().AddRedirect("tasks/(.*)", "todos/$1"));
// Custom middleware to log information about the HTTP Method
app.Use( async (context, next) =>{
    Console.WriteLine($"[ {context.Request.Method} {context.Request.Path} {DateTime.UtcNow} ] Started.");
    await next(context);
    Console.WriteLine($"[ {context.Request.Method} {context.Request.Path} {DateTime.UtcNow} ] Completed.");
});


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

// Start server
app.Run();

// [ Record]: Simple immutable data model
public record Todo(int id, string name, DateTime dueDate, bool isCompleted);
