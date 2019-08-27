using System.Collections.Generic;
using Abp.Configuration;

namespace CROPS.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application, isVisibleToClients: true),
            };
        }
    }
}
