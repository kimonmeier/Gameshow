using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameshow.Shared.Configuration
{
    /// <summary>
    /// Diese Datei enthält alle Extension Methods für die Konfiguration
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Diese Methode lädt die Konfiguration mit Basis-Werten
        /// </summary>
        /// <param name="configurationBuilder">Der Build welcher erstellt wurde</param>
        /// <returns></returns>
        public static IConfiguration LoadBasisConfiguration(this ConfigurationBuilder configurationBuilder) 
            => configurationBuilder
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.dev.json", true, true)
                .Build();
    }
}
