using Newtonsoft.Json;
using System.Text;

namespace HttpCall
{
    public interface IHttpBase
    {
        Task<BaseResponse<T>> Get<T>(string uri);
        Task<BaseResponse<R>> Post<R, S>(string uri, S Element);
        Task<BaseResponse<R>> Put<R, S>(string uri, S Element);
        Task<BaseResponse<T>> Delete<T>(string uri);
    }

    public class BaseResponse<T>
    {
        public string Message { get; set; }
        public T Result { get; set; }
    }

    public class HttpBase : IHttpBase
    {
        private readonly HttpClient _httpClient;

        protected HttpBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResponse<R>> Get<R>(string uri)
        {

            var response = await _httpClient.GetAsync(uri);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BaseResponse<R>>(responseStream);
            }
            else
            {
                throw new Exception($"Error to get the query {uri} Status:{response.StatusCode} Content:{response.Content}");
            }

        }

        public async Task<BaseResponse<R>> Post<R, S>(string uri, S Element)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(Element), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BaseResponse<R>>(responseStream);
            }
            else
            {
                throw new Exception($"Error to get the query {uri} Status:{response.StatusCode} Content:{response.Content}");
            }

        }

        public async Task<BaseResponse<R>> Put<R, S>(string uri, S Element)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(Element), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(uri, content);

            response.EnsureSuccessStatusCode();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject
                    <BaseResponse<R>>(responseStream);
            }
            else
            {
                throw new Exception($"Error to get the query {uri} Status:{response.StatusCode} Content:{response.Content}");
            }

        }

        public async Task<BaseResponse<R>> Delete<R>(string uri)
        {
            var response = await _httpClient.DeleteAsync(uri);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject
                    <BaseResponse<R>>(responseStream);
            }
            else
            {
                throw new Exception($"Error to get the query {uri} Status:{response.StatusCode} Content:{response.Content}");
            }
        }
    }
}
