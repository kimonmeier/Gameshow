using Gameshow.Shared.Services;

namespace Gameshow.Desktop.ViewModel.Base.Commands;

public abstract class AsyncTypeSafeCommand<T> : TypeSafeCommandBase<T> where T : BindableBase
{
    private bool isExecuting;

    protected override bool CanExecute(T parameter)
    {
        return !isExecuting && ShouldExecute(parameter);
    }

    protected override void Execute(T parameter)
    {
        ContinueAsync(parameter).SafeFireAndForget();
    }

    protected abstract bool ShouldExecute(T parameter);

    protected abstract Task ExecuteAsync(T parameter);

    private async Task ContinueAsync(T? parameter)
    {
        if (ShouldExecute(parameter))
        {
            try
            {
                isExecuting = true;
                await ExecuteAsync(parameter);
            }
            finally
            {
                isExecuting = false;
            }
        }

        RaiseCanExecuteChanged();
    }
}