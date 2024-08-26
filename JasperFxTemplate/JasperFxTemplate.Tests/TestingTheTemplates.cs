using Microsoft.TemplateEngine.Authoring.TemplateVerifier;
using Xunit.Abstractions;

namespace JasperFxTemplate.Tests;

public class TestingTheTemplates(ITestOutputHelper output) :TestBase
{
    [Fact]
    public async Task NoOptions()
    {
        
        var templatePath = GetTestTemplateLocation("JasperFxTemplate");
        var options = new TemplateVerifierOptions("critterapi")
        {
            DisableDiffTool = true,
            TemplatePath = templatePath,
            ScenarioName = "Empty"
            
            
        };
        
        
        using var logger  = output.BuildLogger();
        var engine = new VerificationEngine(logger);
        await engine.Execute(options).ConfigureAwait(false);
    }
    [Fact]
    public async Task PortAssignment()
    {
        var templatePath = GetTestTemplateLocation("JasperFxTemplate");
 
        var options = new TemplateVerifierOptions("critterapi")
        {
            DisableDiffTool = true,
            TemplatePath = templatePath,
            ScenarioName = "PortAssignment",
            TemplateSpecificArgs = new [] { "--postgresPort", "9999"}
            
            
        };
        
        
        using var logger  = output.BuildLogger();
        var engine = new VerificationEngine(logger);
        await engine.Execute(options).ConfigureAwait(false);
    }
}