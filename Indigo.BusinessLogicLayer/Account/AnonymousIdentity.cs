namespace Indigo.BusinessLogicLayer.Account
{
    public class AnonymousIdentity : IndigoUserIdentity
    {
        public AnonymousIdentity() : base(null)
        {
        }
    }
}