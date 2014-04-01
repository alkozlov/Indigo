namespace Indigo.DesktopClient.Helpers
{
    using System;
    using System.Collections.Generic;
    using Indigo.DesktopClient.View;

    public class NavigationHistoryService
    {
        private static NavigationHistoryService _current;

        public static NavigationHistoryService Current
        {
            get { return _current ?? (_current = new NavigationHistoryService()); }
        }

        private readonly Stack<ApplicationView> _navigationHistory;

        private NavigationHistoryService()
        {
            this._navigationHistory = new Stack<ApplicationView>();
        }

        public void SaveNavigationAction(ApplicationView applicationView)
        {
            if (!this.IsHistoryEmpty)
            {
                ApplicationView lastItem = this._navigationHistory.Peek();
                if (lastItem != applicationView)
                {
                    this._navigationHistory.Push(applicationView);
                }
            }
            else
            {
                this._navigationHistory.Push(applicationView);
            }
        }

        public ApplicationView ExtractLastNavigationAction()
        {
            return this._navigationHistory.Count > 0 ? this._navigationHistory.Pop() : ApplicationView.Unknown;
        }

        public Boolean IsHistoryEmpty
        {
            get { return this._navigationHistory.Count <= 0; }
        }
    }
}
