using System;
using System.Threading.Tasks;

namespace EvolveVideos.Clients.Services
{
    public interface ILogService
    {
        Task LogExceptionAsync(Exception exception);
    }
}