
using DotNet8.SignalRNotification.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DotNet8.SignalRNotification.WebApi
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IHubContext<StockHub> _stockHub;
        private const string stockName = "Basic stock Name";
        private decimal stockPrice = 100;

        public Worker(ILogger logger, IHubContext<StockHub> stockHub)
        {
            _logger = logger;
            _stockHub = stockHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Worker running at:{time}",DateTimeOffset.Now);
                    Random rn=new Random();
                    decimal stockRaise = rn.Next(0, 10000);
                    string[] stockNames = { "Apple", "Google", "OpenAi", "Microsoft", "Amazon" };
                    var stockName = stockNames[rn.Next(0,stockNames.Length)];
                    await _stockHub.Clients.All.SendAsync("Receive StockPrice", stockName, stockPrice);
                    _logger.LogInformation("Send stock price: {stockName} - {stockRaise}",stockName,stockRaise);
                    await Task.Delay(4000,stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending stock price");
                }
            }
        }
    }
}
