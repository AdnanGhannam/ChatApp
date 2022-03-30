namespace ChatVia.Client.Services
{
    public interface IFetchService
    {
        Task GetAsync<T>(string url,
            Dictionary<string, string>? headers = null,
            string? customClient = null,
            bool includeCredentials = true,
            Action<T>? callback = null);

        Task PostAsync<T>(string url,
            Dictionary<string, string>? headers = null,
            object? body = null,
            string? customClient = null,
            bool includeCredentials = true,
            Action<T>? callback = null);

        Task DeleteAsync<T>(string url,
            Dictionary<string, string>? headers = null,
            object? body = null,
            string? customClient = null,
            bool includeCredentials = true,
            Action<T>? callback = null);
    }
}
