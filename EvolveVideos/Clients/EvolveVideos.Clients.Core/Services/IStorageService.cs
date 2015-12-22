using EvolveVideos.Clients.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvolveVideos.Clients.Core.Services
{
    public interface IStorageService
    {
        Task<List<EvolveSession>> LoadSessions();
    }
}