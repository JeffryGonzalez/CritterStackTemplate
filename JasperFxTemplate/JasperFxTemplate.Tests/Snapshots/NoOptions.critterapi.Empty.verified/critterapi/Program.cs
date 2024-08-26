
using critterapi;
using Oakton;
using Wolverine.Http;

var builder = WebApplication.CreateBuilder(args);

builder.AddCritterStackWebHost("CritterStack");

var app = builder.Build();

app.MapWolverineEndpoints();

await app.RunOaktonCommands(args);
