namespace Saving
{
	public interface ISaveManager
	{
		bool HasSavedData { get; }
		SettingsData SettingsData { get; }

		void SetVolume(float volume);
		void OverrideSaveData(SaveData saveData);

		void SaveData();
		void RestoreData();
		void DeleteData();
	}
}