// See https://aka.ms/new-console-template for more information
using Amazon.CloudWatchLogs;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.AwsCloudWatch;

Console.WriteLine("AWS CloudWatch Logs - Serilog Example");

// Create the AWS CloudWatch Logs Client
/* 

NOTE: this is an AWS SDK object and requires a credential configuration to be in
place before it can be run.

See here for more info: https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/creds-assign.html

The approach used to test this was #6 referenced above where the Access Key and Secret Key 
are configured as env vars.

*/
var client = new AmazonCloudWatchLogsClient(Amazon.RegionEndpoint.EUWest1);

// Create the Serilog Logger
var log = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo
        .File(new CompactJsonFormatter(), "test.log", rollingInterval: RollingInterval.Day)
    .WriteTo
        .AmazonCloudWatch(
            "dev/poc/",
            @"cloud-watch-serilog",
            cloudWatchClient: client,
            textFormatter: new CompactJsonFormatter()       // use this for json objects in the logs
        )
    .CreateLogger();


log.Information("This an informational log message");
log.Information("This is another log message with parameter: {p1} and another parameter: {p2}", Environment.MachineName, Thread.CurrentThread.ManagedThreadId);
log.Debug("This is a debug message");
log.Error(new ApplicationException("My custom exception"), "This is a custom exception");


System.Console.WriteLine("Done");
Console.ReadLine();