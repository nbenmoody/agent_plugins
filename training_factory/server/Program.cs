using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Net;        // WebUtility.HtmlEncode

var builder = Host.CreateApplicationBuilder(args);

//avoid stdout because that is used for the mcp server
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly(); // finds classes with the McpServerToolType attribute.

await builder.Build().RunAsync();

// One section of a training: a heading plus trusted HTML the caller supplies.
public record TrainingSection(string Title, string Body);

[McpServerToolType]
public static class TrainingTools
{
    [McpServerTool, Description("""
        Generate an HTML training in the standard format.
        Use when the user asks to create a training, lesson, or study guide.
        Keep it to 3-5 sections so the result stays a 5-10 minute read.
        Returns the path to the written .html file.
        """)]
    public static string CreateTraining(
        [Description("The training title, shown as the H1.")] string title,
        [Description("Between 3 and 5 sections, each with a title and HTML body.")] TrainingSection[] sections,
        [Description("Directory to write the file into.")] string outDir = ".")
    {
        if (sections.Length is < 3 or > 5)
            throw new ArgumentException("Provide between 3 and 5 sections.");

        var cards = string.Concat(sections.Select((s, i) => RenderSection(i + 1, s)));

        // template ships next to the built app (see the .csproj copy rule)
        var templatePath = Path.Combine(AppContext.BaseDirectory, "templates", "training.html");
        var doc = File.ReadAllText(templatePath)
            .Replace("__TITLE__", WebUtility.HtmlEncode(title))
            .Replace("__SECTIONS__", cards);

        var slug = title.ToLowerInvariant().Replace(" ", "-");
        var path = Path.Combine(outDir, $"training-{slug}.html");
        File.WriteAllText(path, doc);
        return $"Wrote training to {Path.GetFullPath(path)}";
    }

    // Turn one section into a card, matching the house format. Not a tool.
    private static string RenderSection(int i, TrainingSection sec) => $$"""
          <section class="card" data-sec="{{i}}">
            <div class="sec-head"><span class="sec-num">Section {{i}}</span></div>
            <h2>{{WebUtility.HtmlEncode(sec.Title)}}</h2>
            {{sec.Body}}
            <div class="mark"><label><input type="checkbox"> Mark this section complete</label></div>
          </section>
        """;
}