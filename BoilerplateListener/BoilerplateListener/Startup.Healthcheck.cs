using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoilerplateListener
{
    public partial class Startup
    {
        public void AddHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks().AddSqlServer(Configuration["SQL_CON_STRINGS"]);
        }
    }
}
