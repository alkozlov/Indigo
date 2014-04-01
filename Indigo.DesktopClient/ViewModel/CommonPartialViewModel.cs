namespace Indigo.DesktopClient.ViewModel
{
    using System;

    public abstract class CommonPartialViewModel : CommonViewModel
    {
        public override Boolean IsPartialView
        {
            get { return true; }
        }
    }
}