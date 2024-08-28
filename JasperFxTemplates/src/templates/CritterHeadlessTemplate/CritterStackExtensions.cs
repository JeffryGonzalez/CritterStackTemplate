using Oakton;
using Wolverine;

namespace CritterHeadlessTemplate;

public static class CritterStackExtensions
{
    private static string _serviceName = "";
    public static IHostBuilder AddCritterStackHeadless(this IHostBuilder builder, string connectionStringKey)
    {
        builder.ApplyOaktonExtensions();
        // Todo: Need a lot of thought / help here.
        return builder;
    }
}