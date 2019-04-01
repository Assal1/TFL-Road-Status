namespace RoadStatus.Core.Models
{
    /// <summary>
    /// Represents the status of the road
    /// </summary>
    public class RoadStatusResponse
    {
        public bool IsRoadFound { get; set; }

        public string DisplayName { get; set; }

        public string StatusSeverity { get; set; }

        public string StatusSeverityDescription { get; set; }
    }
}
