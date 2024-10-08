/*
 * El código fuente de este archivo es propiedad intelectual de Gerzon Gonzalez.
 */
namespace Viewer;

/**
 * @author gerzon
 */
internal static class Program
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FormPlot());
    }
}