using Sentry;

namespace Gameshow.Shared.Events.Base
{
    /// <summary>
    /// Dieses Event setzt die Antwort
    /// </summary>
    public class EventAnswer : IRequest
    {
        /// <summary>
        /// Die ID es Event
        /// </summary>
        public required Guid EventGuid { get; init; }

        /// <summary>
        /// Der Typ welcher die Antwort beschreibt
        /// </summary>
        public required string AnswerTypeFullName { get; init; }

        /// <summary>
        /// Der Sentry Trace Header um die Requests im Nachhinein nachvollziehen zu können
        /// </summary>
        public required string SentryTraceHeader { get; init; }
        
        /// <summary>
        /// Die Antwort welche gesetzt werden soll
        /// </summary>
        public required string Answer { get; init; }
    }
}
