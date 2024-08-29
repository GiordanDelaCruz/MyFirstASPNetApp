// Provides us with APIs that we can use for our host
var builder = WebApplication.CreateBuilder(args);

// Allow us to make route handlers
var app = builder.Build();

// Handle Get request
app.MapGet("/", () => "Hello World!");

// Start server
app.Run();
