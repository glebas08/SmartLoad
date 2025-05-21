namespace SmartLoad.Models
{
    public class LoadingReportModel
    {
        public int SchemeId { get; set; }
        public LoadingScheme Scheme { get; set; }
        public List<Order> Orders { get; set; }
        public List<BlockPlacementStep> PlacementSteps { get; set; }
        public Dictionary<string, float> AxleLoads { get; set; }
        public string ErrorMessage { get; set; }
    }
}
