using CritterHeadlessTemplate;
using Oakton;
using Wolverine;

var host = Host.CreateDefaultBuilder(args);

host.AddCritterStackHeadless("CritterStack");

return await host.RunOaktonCommands(args);