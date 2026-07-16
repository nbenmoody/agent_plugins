#:package UglyToad.PdfPig@0.1.10
// Print pages of exam_guide.pdf that mention a topic.
// Usage: dotnet run read_guide.cs "<topic>"
using UglyToad.PdfPig;

var topic = args.Length > 0 ? args[0].ToLowerInvariant() : "";
using var pdf = PdfDocument.Open("exam_guide.pdf");

int n = 0;
foreach (var page in pdf.GetPages())
{
    n++;
    if (page.Text.ToLowerInvariant().Contains(topic))
    {
        Console.WriteLine($"\n===== page {n} =====");
        Console.WriteLine(page.Text.Trim());
    }
}