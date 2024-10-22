using System.Diagnostics;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TestFuncs;

public class MyBlobTrigger(ILogger<MyBlobTrigger> logger)
{
    private static ActivitySource test = new("testme");
    [Function(nameof(MyBlobTrigger))]
    [BlobOutput("test-files/{name}.txt", Connection = "Storage")]
    public string Run([BlobTrigger("blobs/{name}", Connection = "Storage")] string triggerString)
    {
        using var act = test.StartActivity("THIS IS A TEST");
        logger.LogInformation("C# blob trigger function invoked with {message}...", triggerString.Substring(0, 100));
        logger.LogInformation("{act_id}, {cure_act_name}", act?.Id, Activity.Current?.DisplayName);
        return triggerString.ToUpper();
    }
}
