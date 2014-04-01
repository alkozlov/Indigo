namespace Indigo.DesktopClient
{
    using System;
    using System.Windows;

    using GalaSoft.MvvmLight.Threading;

    using Indigo.BusinessLogicLayer.UserAccount;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //Create a custom principal with an anonymous identity at startup
            IndigoUserPrincipal principal = new IndigoUserPrincipal();
            AppDomain.CurrentDomain.SetThreadPrincipal(principal);

            base.OnStartup(e);
        }
    }
}
