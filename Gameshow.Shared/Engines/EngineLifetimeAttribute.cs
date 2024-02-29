using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameshow.Shared.Engines
{
    /// <summary>
    /// Dieses Attribute markiert wie lange eine Engine bestehen bleibt
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EngineLifetimeAttribute : Attribute
    {
        /// <summary>
        /// Die Lebenszeit der Engine
        /// </summary>
        public ServiceLifetime Lifetime { get; init; }

        /// <summary>
        /// Der Konstruktor welcher die Lifetime entgegennimmt
        /// </summary>
        /// <param name="lifetime">Die angegeben Lifetime</param>
        public EngineLifetimeAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }
}
