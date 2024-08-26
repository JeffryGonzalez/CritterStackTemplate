
using JasperFxTemplate;
using Oakton;
using Wolverine.Http;

var builder = WebApplication.CreateBuilder(args);

builder.UseCritterStackWebHost("CritterStack");


var app = builder.Build();

app.MapWolverineEndpoints();

await app.RunOaktonCommands(args);
