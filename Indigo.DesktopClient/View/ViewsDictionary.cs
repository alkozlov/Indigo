namespace Indigo.DesktopClient.View
{
    using System;
    using System.Collections.Generic;

    using Indigo.DesktopClient.ViewModel;

    public static class ViewsDictionary
    {
        private static Dictionary<ApplicationView, Type> _availableViews = new Dictionary<ApplicationView, Type>
        {
            {ApplicationView.Analysis, typeof(AnalysisViewModel)},

            {ApplicationView.SignIn, typeof(SignInViewModel)}
        };

        public static Type GetViewType(ApplicationView applicationView)
        {
            return _availableViews.ContainsKey(applicationView)
                ? _availableViews[applicationView]
                : null;
        }

        public static Boolean IsMainView(ApplicationView applicationView)
        {
            return applicationView == ApplicationView.Analysis || applicationView == ApplicationView.AnalysisResults
                   || applicationView == ApplicationView.AnalysisSettings || applicationView == ApplicationView.SignIn
                   || applicationView == ApplicationView.SignUp || applicationView == ApplicationView.PasswordRecovery;
        }
    }
}