namespace JasperFxTemplate.Tests;

public class TestBase
{
    private static string CodeBaseRoot { get; } = GetCodeBaseRoot();
    private static string TestTemplatesLocation { get; } = Path.Combine(CodeBaseRoot);
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
        var codebase = typeof(TestBase).Assembly.Location;
        // Are you my mother?
        var codeBaseRoot = new FileInfo(codebase).Directory?.Parent?.Parent?.Parent?.Parent?.FullName;

        if (string.IsNullOrEmpty(codeBaseRoot))
        {
            throw new InvalidOperationException("The codebase root was not found");
        }
        if (!File.Exists(Path.Combine(codeBaseRoot!, "JasperFxTemplate.sln")))
        {
            throw new InvalidOperationException("JasperFxTemplate.sln was not found in codebase root");
        }

        return codeBaseRoot!;
    }
}