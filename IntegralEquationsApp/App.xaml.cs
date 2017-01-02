using IntegralEquationsApp.Data;
using System.Windows;

namespace IntegralEquationsApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            AppData.Initialize();
        }
    }
}
