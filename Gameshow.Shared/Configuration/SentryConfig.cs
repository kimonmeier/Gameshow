using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameshow.Shared.Configuration
{
    /// <summary>
    /// Diese Klasse wiederspiegelt die Sentry Konfiguration
    /// </summary>
    public class SentryConfig
    {
        /// <summary>
        /// Die DSN Adresse für Sentry
        /// </summary>
        public string Dsn { get; set; } = null!;

        /// <summary>
        /// Ob der Debug Modus aktiviert werden soll
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Ob Sentry Aktiv ist!
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Die aktuelle Umgebung von Sentry
        /// </summary>
        public string Environment { get; set; } = null!;

        /// <summary>
        /// Wie oft getracted wird!
        /// </summary>
        public float TraceSampleRate { get; set; }

        /// <summary>
        /// Die Sample Rate
        /// </summary>
        public float SampleRate { get; set; }
    }
}
