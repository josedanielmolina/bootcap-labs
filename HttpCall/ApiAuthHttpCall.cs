using DTO.Event;

namespace HttpCall
{
    public interface IApiAuthHttpCall
    {
        Task<bool> DarAltaUsuario(RequestActivarEmpleado @event);
    }

    public class ApiAuthHttpCall : HttpBase, IApiAuthHttpCall
    {
        public ApiAuthHttpCall(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<bool> DarAltaUsuario(RequestActivarEmpleado @event)
        {
            await Post<RequestActivarEmpleado, object>("DarAltaUsuario", @event);
            return true;
        }
    }
}
