namespace Indigo.DesktopClient.ViewModel
{
    using System;
    using System.Threading;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using Indigo.BusinessLogicLayer.Account;
    using Indigo.DesktopClient.Helpers;
    using Indigo.DesktopClient.Model.Notifications;
    using Indigo.DesktopClient.View;

    public abstract class CommonViewModel : ViewModelBase
    {
        public abstract ApplicationView ViewType { get; }

        public virtual Boolean IsPartialView
        {
            get { return false; }
        }

        public void NavigateAction(ApplicationView fromView, ApplicationView toView, Object navigationToken = null, Boolean isJumpBack = false)
        {
            if (!isJumpBack)
            {
                NavigationHistoryService.Current.SaveNavigationAction(fromView);
            }
            NavigationMessage navigationMessage = new NavigationMessage(toView, isJumpBack);
            if (navigationToken != null)
            {
                Messenger.Default.Send<NavigationMessage>(navigationMessage, navigationToken);
            }
            else
            {
                Messenger.Default.Send<NavigationMessage>(navigationMessage);
            }
        }

        public IndigoUserPrincipal UserPrincipal
        {
            get { return Thread.CurrentPrincipal as IndigoUserPrincipal; }
        }
    }
}
