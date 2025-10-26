using Avalonia;

namespace EvilBaschdi.Testing.Avalonia;

/// <summary>
///     Static class that provides thread-safe initialization of Avalonia applications for testing purposes.
///     Ensures the headless platform is configured once per application type.
/// </summary>
/// <typeparam name="TApp">The type of Avalonia application to initialize</typeparam>
public static class TestAppBuilder<TApp>
    where TApp : Application, new()
{
    // ReSharper disable once StaticMemberInGenericType
    private static bool _isInitialized;

    // ReSharper disable once StaticMemberInGenericType
    private static readonly Lock Lock = new Lock();

    /// <summary>
    ///     Ensures the Avalonia application is initialized for testing with thread-safe double-checked locking.
    ///     This method configures the headless platform if not already initialized.
    /// </summary>
    public static void EnsureInitialized()
    {
        lock (Lock)
        {
            if (_isInitialized)
            {
                return;
            }
        }

        lock (Lock)
        {
            if (_isInitialized)
            {
                return;
            }

            BuildAvaloniaApp().SetupWithoutStarting();
            _isInitialized = true;
        }
    }

    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<TApp>()
                     .UseHeadless(new AvaloniaHeadlessPlatformOptions
                                  {
                                      UseHeadlessDrawing = true
                                  });
}