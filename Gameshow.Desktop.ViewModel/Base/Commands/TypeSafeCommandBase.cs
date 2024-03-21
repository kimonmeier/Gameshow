namespace Gameshow.Desktop.ViewModel.Base.Commands;

public abstract class TypeSafeCommandBase<T> : CommandBase
{
    protected abstract bool CanExecute(T parameter);

    protected abstract void Execute(T parameter);

    public override bool CanExecute(object? parameter)
    {
        return parameter is T @base && CanExecute(@base);
    }

    public override void Execute(object? parameter)
    {
        if (parameter is not T @base)
        {
            return;
        }

        Execute(@base);
    }
}