namespace ECommerceAIMockUp.Application.Services.Interfaces.Caching
{
    public interface IRedisService
    {
        T? GetData<T>(string Key);
        void SetData<T>(string key, T Data);
    }
}
