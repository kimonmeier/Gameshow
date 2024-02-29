using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gameshow.Shared.Events.Base
{
    /// <summary>
    /// Dies ist das Basis Event welches in den Eventbus geschrieben wird!
    /// </summary>
    public class BaseEvent
    {
        /// <summary>
        /// Gibt an ob das Event eine Antwort zurückliefert
        /// </summary>
        public bool HasAnswer { get; set; }

        /// <summary>
        /// Die ID des erstellten Events
        /// </summary>
        public Guid EventGuid { get; set; }

        /// <summary>
        /// Der Request welcher gesendet werden soll
        /// </summary>
        [JsonConverter(typeof(InterfaceConverter<IBaseRequest>))]
        public IBaseRequest Request { get; set; } = null!;
    }
}
