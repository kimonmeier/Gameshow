﻿using System.Windows.Controls;

namespace Gameshow.Desktop.ViewModel.Base;

public abstract class BindableBase : INotifyPropertyChanged
{
    private readonly List<PropertyInfo> commandBases;
    private readonly MethodInfo commandBaseRetriggerExecute;

    protected BindableBase()
    {
        commandBases = GetType().GetProperties().Where(x => x.PropertyType == typeof(CommandBase) || x.PropertyType == typeof(UIElement)).ToList();
        commandBaseRetriggerExecute = typeof(CommandBase).GetMethod(nameof(CommandBase.RaiseCanExecuteChanged))!;
    }

    /// <summary>
    /// Multicast event for property change notifications.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Checks if a property already matches the desired value.  Sets the property and
    /// notifies listeners only when necessary.
    /// </summary>
    /// <typeparam name="T">Type of the property.</typeparam>
    /// <param name="storage">Reference to a property with both getter and setter.</param>
    /// <param name="value">Desired value for the property.</param>
    /// <param name="propertyName">Name of the property used to notify listeners.This
    /// value is optional and can be provided automatically when invoked from compilers that
    /// support CallerMemberName.</param>
    /// <returns>True if the value was changed, false if the existing value matched the
    /// desired value.</returns>
    protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(storage, value)) return false;
        storage = value;
        // Log.DebugFormat("{0}.{1} = {2}", this.GetType().Name, propertyName, storage);
        OnPropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// Notifies listeners that a property value has changed.
    /// </summary>
    /// <param name="propertyName">Name of the property used to notify listeners.  This
    /// value is optional and can be provided automatically when invoked from compilers
    /// that support <see cref="CallerMemberNameAttribute"/>.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (!Application.Current.Dispatcher.CheckAccess())
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                OnPropertyChanged(propertyName);
            });
            return;
        }
        PropertyChangedEventHandler? eventHandler = PropertyChanged;
        eventHandler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        CallCommandsRecursive();
    }

    protected void CallCommandsRecursive()
    {
        foreach (PropertyInfo commandProperty in commandBases)
        {
            SearchRecursive(commandProperty, this);
        }
    }

    public void SearchRecursive(PropertyInfo propertyInfo, object? value)
    {
        if (propertyInfo.PropertyType == typeof(CommandBase))
        {
            commandBaseRetriggerExecute.Invoke(propertyInfo.GetValue(this), []);
            return;
        }
        
        UserControl? userControl = propertyInfo.GetValue(value) as UserControl;
        if (userControl?.DataContext is not BindableBase bindableBase)
        {
            return;
        }

        bindableBase.CallCommandsRecursive();
    }
}