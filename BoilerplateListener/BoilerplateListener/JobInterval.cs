using Logic.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BoilerplateListener
{
    public class JobInterval : BackgroundService
    {
        private readonly ILogger<JobInterval> _logger;
        private readonly IBoilerplateListenerLogic _boilerplateListenerLogic;
        public JobInterval(ILogger<JobInterval> logger, IBoilerplateListenerLogic boilerplateListenerLogic)
        {
            _boilerplateListenerLogic = boilerplateListenerLogic;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"JobInterval is starting.");

            stoppingToken.Register(() =>
                _logger.LogWarning($" JobInterval background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"JobInterval task doing background work.");
                _boilerplateListenerLogic.Job();
            }

            _logger.LogWarning($"JobInterval background task is stopping.");
        }
    }
}
