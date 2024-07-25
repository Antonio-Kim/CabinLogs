using CabinLogsApi.Models;

interface ISettingServie
{
	public Task<List<Setting>> GetSettings();
}