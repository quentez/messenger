using System.Threading.Tasks;

namespace Messenger.Lib.Services
{
    public interface IInjectScriptService
    {
        Task InitializeAsync();
        Task<string> GetScriptAsync();
    }
}