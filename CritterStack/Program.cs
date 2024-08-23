using CritterStack;
using Oakton;
using Wolverine.Http;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.UseCritterStack();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapDefaultEndpoints();

app.UseHttpsRedirection();
app.MapWolverineEndpoints();


await app.RunOaktonCommands(args);