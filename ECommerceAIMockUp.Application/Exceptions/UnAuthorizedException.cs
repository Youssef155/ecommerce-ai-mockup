namespace ECommerceAIMockUp.Application.Exceptions
{
    public class UnAuthorizedException : Exception
    {
        public UnAuthorizedException() : base("Unauthorized access")
        {

        }
    }
}
