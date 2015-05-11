using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CefSharp;

namespace Messenger.Lib.Services
{
    public interface IPreloadCacheService : IResourceHandler
    {
        Task AddAndPreloadResourceAsync(Regex route, Uri resource);
    }
}