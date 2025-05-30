using Microsoft.Extensions.DependencyInjection;
using SyncUpC.Domain.Ports.Configuration.Claims;
using SyncUpC.Infraestructure.Adapters.Configuration.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Infraestructure.Extensions.Claims;

public static class ClaimExtension
{
    public static IServiceCollection AddClaims(this IServiceCollection services)
    {
        services.AddTransient<IClaimService, ClaimService>();
        return services;
    }
}
