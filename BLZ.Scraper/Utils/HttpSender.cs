namespace BLZ.Scraper.Utils;
internal class HttpSender<T>
{
    private readonly HttpClient _httpClient;
    private readonly Func<HttpResponseMessage, Task<T>> _responseHandler;
    public HttpSender(HttpClient httpClient, Func<HttpResponseMessage, Task<T>> responseHandler)
    {
        _httpClient = httpClient;
        _responseHandler = responseHandler;
    }

    public async Task<T> SendAsync(HttpRequestMessage req)
    {
        var response = await _httpClient.SendAsync(req);
        return await _responseHandler(response);
    }
}