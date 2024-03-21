namespace Gameshow.Desktop.ViewModel.Base.Commands;

public abstract class CommandBase: ICommand
{
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public abstract bool CanExecute(object? parameter);

    public abstract void Execute(object? parameter);

    public event EventHandler? CanExecuteChanged;
}