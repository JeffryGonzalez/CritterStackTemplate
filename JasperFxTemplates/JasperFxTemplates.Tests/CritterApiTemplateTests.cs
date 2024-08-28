using Microsoft.TemplateEngine.Authoring.TemplateVerifier;
using Xunit.Abstractions;

namespace JasperFxTemplates.Tests;

public class CritterApiTemplateTests(ITestOutputHelper output) :TestBase
{
    [Fact]
    public async Task NoOptions()
    {
        
        var templatePath = GetTestTemplateLocation("CritterApiTemplate");
        var options = new TemplateVerifierOptions("critterapi")
        {
            DisableDiffTool = true,
            TemplatePath = templatePath,
            ScenarioName = "Empty",
            TemplateSpecificArgs = ["-n" ,"Empty"]


        };
        
        
        using var logger  = output.BuildLogger();
        var engine = new VerificationEngine(logger);
        await engine.Execute(options);
    }
    [Fact]
    public async Task PortAssignment()
    {
        var templatePath = GetTestTemplateLocation("CritterApiTemplate");
 
        var options = new TemplateVerifierOptions("critterapi")
        {
            DisableDiffTool = true,
            TemplatePath = templatePath,
            ScenarioName = "PortAssignment",
            TemplateSpecificArgs = ["-n", "PortAssignment", "--postgresPort", "9999"]


        };
        
        
        using var logger  = output.BuildLogger();
        var engine = new VerificationEngine(logger);
        await engine.Execute(options);
    }
    [Fact]
    public async Task DatabaseName()
    {
        var templatePath = GetTestTemplateLocation("CritterApiTemplate");
 
        var options = new TemplateVerifierOptions("critterapi")
        {
            DisableDiffTool = true,
            TemplatePath = templatePath,
            ScenarioName = "PortAssignment",
            TemplateSpecificArgs = ["-n", "DatabaseName", "--databaseName", "killing-time"]


        };
        
        
        using var logger  = output.BuildLogger();
        var engine = new VerificationEngine(logger);
        await engine.Execute(options);
    }
}