namespace Gameshow.Desktop.View;

public static class DependencyInjectionLocator
{
    private static IServiceProvider? _provider; 
    
    public static IServiceProvider Provider
    {
        get
        {
            if (_provider is null)
            {
                throw new Exception("Dependency Injection not initialized");
            }

            return _provider;
        }
        set
        {
            if (_provider is not null)
            {
                throw new Exception("Dependency Injection already initialized");
            }

            _provider = value;
        }
    }
}