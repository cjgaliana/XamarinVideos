using System.Threading.Tasks;

namespace EvolveVideos.Clients.Core.Utils
{
    public static class TaskUtils
    {
        public static Task CompletedTask => Task.FromResult(0);

        public static void FireAndForget(this Task task)
        {
            
        }
    }
}