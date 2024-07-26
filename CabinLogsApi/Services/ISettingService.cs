using CabinLogsApi.Models;

public interface ISettingService
{
	public Task<List<Setting>> GetSettings();
}