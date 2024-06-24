using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using MessengerApp.Interfaces;

namespace MessengerApp.BackgroundServices
{
    public class ChatBackgroundService : BackgroundService
    {
        private readonly ILogger<ChatBackgroundService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ChatBackgroundService(ILogger<ChatBackgroundService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ChatBackgroundService is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation("ChatBackgroundService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("ChatBackgroundService is doing background work.");

                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
                        await messageService.CleanUpOldMessagesAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while performing background tasks.");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); 
            }

            _logger.LogInformation("ChatBackgroundService has stopped.");
        }
    }
}
