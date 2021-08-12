using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoilerplateListener
{
    public partial class Startup
    {
        private void ConfigureLogs(IServiceCollection services)
        {
            services.AddLogging(x => x.ClearProviders());
            services.AddLogging(x => x.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(Configuration).Enrich.FromLogContext().CreateLogger()));

        }
    }
}
