using Microsoft.Extensions.Localization;
using SyncUpC.Domain.Ports.Configuration.Localization;
using SyncUpC.Infraestructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Infraestructure.Adapters.Configuration.Localization;

public class LocalizationService : ILocalizationService
{
    private readonly IStringLocalizer _localizer;

    public LocalizationService(IStringLocalizerFactory factory)
    {
        var assembly = Assembly.Load(ProjectConstant.DomainProject);
        var assemblyName = new AssemblyName(assembly.FullName!);
        _localizer = factory.Create("Common.Resources.SharedResource", assemblyName.Name!);
    }

    public string GetLocalizedByKey(string key)
    {
        return _localizer[key].Value;
    }
}
