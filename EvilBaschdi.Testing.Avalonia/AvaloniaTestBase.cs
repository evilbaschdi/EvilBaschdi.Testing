using Avalonia;

namespace EvilBaschdi.Testing.Avalonia;

/// <summary>
///     Base class for Avalonia tests that ensures the headless platform is initialized
/// </summary>
public abstract class AvaloniaTestBase<TApp>
    where TApp : Application, new()
{
    /// <summary>
    ///     Constructor
    /// </summary>
    protected AvaloniaTestBase()
    {
        TestAppBuilder<TApp>.EnsureInitialized();
    }
}