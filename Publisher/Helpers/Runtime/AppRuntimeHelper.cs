using System;
using System.Text;

namespace Helpers.Runtime
{
    public static class AppRuntimeHelper
    {
        public static void ExitConsoleApp()
        {
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
