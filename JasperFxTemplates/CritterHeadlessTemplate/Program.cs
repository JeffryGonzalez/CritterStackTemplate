using Oakton;
using Wolverine;

return await Host.CreateDefaultBuilder(args)
    .UseWolverine(opts =>
    {
        
    }).RunOaktonCommands(args);