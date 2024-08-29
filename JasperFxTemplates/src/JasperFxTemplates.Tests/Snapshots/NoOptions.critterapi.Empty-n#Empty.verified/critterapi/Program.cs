using Empty;
using Oakton;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.AddCritterStackWebHost("CritterStack");

var app = builder.Build();

app.MapWolverineEndpoints(opts =>
{
    opts.UseFluentValidationProblemDetailMiddleware();
});

return await app.RunOaktonCommands(args);
