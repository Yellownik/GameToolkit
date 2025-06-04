using GameManagers;

namespace Saving
{
	public interface ISaveManager
	{
		bool HasSavedData { get; }
		SettingsData SettingsData { get; }
		BeautySaveData BeautySaveData { get; }

		void SetVolume(float volume);
		void SetCoinsCount(int value);
		void OverrideSaveData(SaveData saveData);

		void SaveData();
		void RestoreData();
		void DeleteData();
	}
}