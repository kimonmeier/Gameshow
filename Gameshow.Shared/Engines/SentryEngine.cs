using Microsoft.Extensions.DependencyInjection;
using NLog;
using Sentry;

namespace Gameshow.Shared.Engines
{
    /// <summary>
    ///     Die Sentry Engine welche
    /// </summary>
    [EngineLifetime(ServiceLifetime.Transient)]
    public sealed class SentryEngine : IEngine, IDisposable
    {
        #region Fields

        private readonly SentryEngine? parentEngine;
        private ISpan? currentChild;
        private SentryEngine? currentChildEngine;
        private ITransaction? currentTransaction;

        #endregion

        #region Constructor

        /// <summary>
        ///     DI Constructor
        /// </summary>
        public SentryEngine()
        {
        }

        private SentryEngine(SentryEngine parent)
        {
            parentEngine = parent;
        }

        #endregion

        #region Methods

        /// <inheritdoc cref="SentrySdk.CaptureException(System.Exception)" />
        public void CaptureException(Exception e)
        {
            if (!SentrySdk.IsEnabled)
            {
                return;
            }

            if (currentTransaction != null)
            {
                currentTransaction.Finish(e);
            } else if (parentEngine != null)
            {
                parentEngine.CaptureException(e);
            } else
            {
                LogManager.GetCurrentClassLogger().Warn("Parent and current Transaction not found, something is wrong!");
                SentrySdk.CaptureException(e);
            }

            FlushTransaction();
        }

        /// <summary>
        ///     Starte eine Sentry Transaktion
        /// </summary>
        /// <param name="operation">Die Operation welche durchgeführt wird</param>
        /// <param name="description">Die Beschreibung</param>
        /// <exception cref="Exception">Die Exception welche geworfen wird, wenn bereits eine Transaktion im Gange ist</exception>
        public void StartTransaction(string operation, string description)
        {
            if (!SentrySdk.IsEnabled)
            {
                return;
            }

            if (currentTransaction is not null)
            {
                throw new Exception("Transaction is already started!");
            }

            currentTransaction = SentrySdk.StartTransaction(operation, description);
        }

        /// <summary>
        ///     Startet eine Transaktion als Child
        /// </summary>
        /// <param name="operation">Die Operation welche durchgeführt wird</param>
        /// <param name="description">Die Beschreibung</param>
        /// <returns>Gibt eine neue SentryEngine zurück welche als Child gespeichert wird</returns>
        /// <exception cref="Exception">Sobald eine unerwartete Situation auftritt</exception>
        public SentryEngine StartChild(string operation, string description)
        {
            if (!SentrySdk.IsEnabled)
            {
                return new SentryEngine();
            }

            if (currentTransaction is null && parentEngine is null)
            {
                throw new Exception("Transaction is not started!");
            }

            if (currentChild is not null)
            {
                throw new Exception("Child is already started!");
            }

            SentryEngine engine = new(this);
            currentChildEngine = engine;
            currentChild = engine.StartTransactionFromParent(operation, description, currentTransaction, currentChild);

            return engine;
        }

        /// <summary>
        /// </summary>
        public void FlushChild()
        {
            if (!SentrySdk.IsEnabled)
            {
                return;
            }

            FlushTransactionFromParent(this);
        }

        /// <summary>
        ///     Endet alle offenen Transaktionen und Child Transaktion
        /// </summary>
        /// <exception cref="Exception">Wirft eine Exception bei einem unerwarteten State</exception>
        public void FlushTransaction()
        {
            if (!SentrySdk.IsEnabled)
            {
                return;
            }

            FlushChild();

            if (currentTransaction is null)
            {
                return;
            }

            currentTransaction.Finish();
            currentTransaction = null;
        }

        private ISpan? StartTransactionFromParent(string operation, string description, ITransaction? parentTransaction, ISpan? parentSpan)
        {
            if (!SentrySdk.IsEnabled)
            {
                return null;
            }

            if (parentTransaction is not null)
            {
                currentChild = parentTransaction.StartChild(operation, description);
            }
            else if (parentSpan is not null)
            {
                currentChild = parentSpan.StartChild(operation, description);
            }
            else
            {
                throw new Exception("The Transaction and Span was not defined");
            }

            return currentChild;
        }

        private static void FlushTransactionFromParent(SentryEngine engine)
        {
            if (!SentrySdk.IsEnabled)
            {
                return;
            }

            if (engine.currentChild is null)
            {
                throw new Exception("The child is not set");
            }

            if (engine.currentChildEngine is not null)
            {
                engine.currentChildEngine.FlushChild();
            }

            engine.currentChild.Finish();

            engine.currentChild = null;
            engine.currentChildEngine = null;
        }


        /// <inheritdoc/>
        public void Dispose()
        {
            FlushTransaction();
        }

        #endregion
    }
}
