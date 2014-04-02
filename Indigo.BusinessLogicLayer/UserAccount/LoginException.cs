namespace Indigo.BusinessLogicLayer.UserAccount
{
    using System;

    public enum LoginExceptionReason
    {
        EmailOrPasswordInvalid,
		AccountDisabled
    }

    public class LoginException : ApplicationException
    {
        public LoginExceptionReason Reason { get; private set; }

        public LoginException(LoginExceptionReason reason)
        {
            this.Reason = reason;
        }

        public LoginException(LoginExceptionReason reason, String message) : base(message)
        {
            this.Reason = reason;
        }
    }
}