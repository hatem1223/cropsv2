using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CROPS.Tests.Mock
{
    public class ConfigurationMock : IConfiguration
    {
        public string this[string key] { get { return "http://localhost:5000/"; } set => throw new NotImplementedException(); }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            return new ConfigurationSectionMock();
        }

        IChangeToken IConfiguration.GetReloadToken()
        {
            throw new NotImplementedException();
        }
    }
}
