using Newtonsoft.Json;

namespace Messenger.Lib.Services
{
    class JsonService : IJsonService
    {
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}