using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("Starting Project");

string hubUrl = "https://localhost:7069/hubs/stock";
var hubConnection = new HubConnectionBuilder()
              .WithUrl(hubUrl)
              .Build();
hubConnection.On<string, decimal>("ReceiveStockPrice", (stockName, stockPrice) =>
{
    Console.WriteLine($"MessageReceive => StockName:{stockName}  StockPrice:{stockPrice}");
});

try
{
    hubConnection.StartAsync().Wait();
    Console.WriteLine("SignalR Connection Start");
}
catch(Exception ex)
{
    Console.WriteLine($"Error connection: {ex.ToString()}");
    throw;
}
CancellationTokenSource cts=new CancellationTokenSource();
var cancellationToken=cts.Token;

Console.CancelKeyPress += (sender, a) =>
{
    a.Cancel = true;
    Console.WriteLine("SignalR Connection Stop");
    cts.Cancel();
};
try
{
    await Task.Delay(Timeout.Infinite, cancellationToken);
}
catch (TaskCanceledException) { }

Console.WriteLine("Signal R Connection Close");