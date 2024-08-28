using CritterHeadless;
using Oakton;
using Wolverine;

var host = Host.CreateDefaultBuilder(args);

host.AddCritterStackHeadless("CritterStack");

await host.RunOaktonCommands(args);