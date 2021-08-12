using App.Metrics;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoilerplateListener
{
    public partial class Startup
    {
        private void ConfigureMetrics(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(option => { option.AllowSynchronousIO = true; });
            services.AddMetrics(CreateMetrics());
            services.AddMetricsTrackingMiddleware();
            services.AddMetricsEndpoints(x => x.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter());
        }

        private IMetricsRoot CreateMetrics()
        {
            return AppMetrics.CreateDefaultBuilder().Configuration.Configure(option =>
            {
                option.Enabled = true;
                option
                .ReportingEnabled = true;
            }).Build();
        }
    }
}
