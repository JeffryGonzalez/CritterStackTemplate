namespace JasperFxTemplate.Tests;

public class TestBase
{
    internal static string CodeBaseRoot { get; } = GetCodeBaseRoot();
    internal static string TestTemplatesLocation { get; } = Path.Combine(CodeBaseRoot);
    internal static string GetTestTemplateLocation(string templateName)
    {
        string templateLocation = Path.Combine(TestTemplatesLocation, templateName);

        if (!Directory.Exists(templateLocation))
        {
            throw new Exception($"{templateLocation} does not exist");
        }
        return Path.GetFullPath(templateLocation);
    }
    private static string GetCodeBaseRoot()
    {
        string codebase = typeof(TestBase).Assembly.Location;
        string? codeBaseRoot = new FileInfo(codebase).Directory?.Parent?.Parent?.Parent?.Parent?.FullName;

        if (string.IsNullOrEmpty(codeBaseRoot))
        {
            throw new InvalidOperationException("The codebase root was not found");
        }
        if (!File.Exists(Path.Combine(codeBaseRoot!, "JasperFxTemplate.sln")))
        {
            throw new InvalidOperationException("JasperFxTemplate.sln was not found in codebase root");
        }
        // if (!Directory.Exists(Path.Combine(codeBaseRoot!, "test", "Microsoft.TemplateEngine.TestTemplates")))
        // {
        //     throw new InvalidOperationException("Microsoft.TemplateEngine.TestTemplates was not found in test/");
        // }
        return codeBaseRoot!;
    }
}