using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.PerformanceScaling20241122.Documents;
using QuestPDF.PerformanceScaling20241122.Models;

QuestPDF.Settings.License = LicenseType.Community;

var numberOfRenders = 5;

void GeneratePDF(int threadNumber)
{
    var model = DocumentModelGenerator.GenerateLargeShallowModel();
    //var document = new DynamicDocument(model);
    var document = new RegularDocument(model);

    var bytes = document.GeneratePdf();
}

Task StartTask(int threadNumber)
{
    var task = Task.Run(() => GeneratePDF(threadNumber));

    return task;
}

void ExecuteMultithreaded()
{
    var tasks = new List<Task>();

    for (var i = 0; i < numberOfRenders; i++)
    {
        var task = StartTask(i + 1);

        tasks.Add(task);
    }

    Task.WaitAll(tasks.ToArray());
}

void ExecuteSequential()
{
    for (var i = 0; i < numberOfRenders; i++)
    {
        var startTime = DateTime.Now;
        GeneratePDF(i + 1);
        Console.WriteLine($"Time elapsed ({i + 1}): " + (DateTime.Now - startTime).ToString(@"hh\:mm\:ss"));
    }
}

var startTime = DateTime.Now;

//ExecuteMultithreaded();
ExecuteSequential();

var endTime = DateTime.Now;
var timeElapsed = endTime - startTime;

Console.WriteLine("Time elapsed: " + timeElapsed.ToString(@"hh\:mm\:ss"));

Console.WriteLine("Press Enter");
Console.ReadLine();