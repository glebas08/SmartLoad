using SmartLoad.Models;

namespace SmartLoad.Services
{
    public interface IReportService
    {
        Task<byte[]> GenerateWordAsync(LoadingReportModel model);
    }
}
