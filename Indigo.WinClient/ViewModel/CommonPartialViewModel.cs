using System;

namespace Indigo.WinClient.ViewModel
{
    public class CommonPartialViewModel : CommonViewModel
    {
        public override Boolean IsPartialView
        {
            get { return true; }
        }
    }
}