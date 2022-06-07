using LX;
using Meteo.Views;

namespace Meteo.Desktop
{
    class Program
    {
        static void Main(string[] args)
        {
            App.OnRun += () => new MainForm();
            App.Run();
        }
    }
}
