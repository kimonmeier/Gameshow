using Gameshow.Shared.Services;

namespace Gameshow.Desktop.ViewModel.Base.Commands;

public abstract class AsyncCommand : CommandBase
{
    private bool isExecuting;

    public override bool CanExecute(object? parameter)
    {
        return !isExecuting && ShouldExecute(parameter);
    }

    public override void Execute(object? parameter)
    {
        ContinueAsync(parameter).SafeFireAndForget();
    }

    protected abstract bool ShouldExecute(object? parameter);

    protected abstract Task ExecuteAsync(object? parameter);

    private async Task ContinueAsync(object? parameter)
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