using RoadStatus.Core.Models;
using System.Threading.Tasks;

namespace RoadStatus.Core.Interfaces
{
    public interface IRoadStatusClient
    {
        Task<RoadStatusResponse> GetRoadStatusAsync(string roadId);
    }
}
