using CWTest.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace DataAccess.Excel.Context
{
    public interface IExcelConfiguration : IConfiguration
    {
      
    }

    public class ExcelConfiguration : IExcelConfiguration
    {
        private IConfiguration _configuration { get; }

        public string this[string key] { get => _configuration[key]; set => _configuration[key] = value; }

        public ExcelConfiguration()
        {
            _configuration = ConfigHelper.Configure(@"ExcelConfig.json");
        }

        public IConfigurationSection GetSection(string key)
        {
            return _configuration.GetSection(key);
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return _configuration.GetChildren();
        }

        public IChangeToken GetReloadToken()
        {
            return _configuration.GetReloadToken();
        }
    }
}