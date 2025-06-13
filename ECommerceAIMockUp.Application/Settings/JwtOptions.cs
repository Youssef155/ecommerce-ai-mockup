namespace ECommerceAIMockUp.Application.Settings
{
    public class JwtOptions
    {
        public string Key { get; set; } = default!;
        public int ExpiredInDays { get; set; } = default!;
        public string Issuer { get; set; } = default!;
    }
}
