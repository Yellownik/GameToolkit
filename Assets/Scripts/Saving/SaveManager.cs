using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GameManagers;
using UnityEngine;

namespace Saving
{
	public class SaveManager : ISaveManager
	{
		private readonly string SavePath = Application.persistentDataPath + "/data.save";
		private SaveData CachedData;

		public bool HasSavedData => File.Exists(SavePath);

		public SettingsData SettingsData => CachedData.SettingsData;
		public BeautySaveData BeautySaveData => CachedData.BeautySaveData;

		public void SetVolume(float volume)
		{
			CachedData.SettingsData.Volume = volume;
		}

		public void SetCoinsCount(int value)
		{
			CachedData.BeautySaveData.CoinsCount = value;
		}
		
		public void OverrideSaveData(SaveData saveData)
		{
			CachedData = saveData;
			WriteDataOnStorage();
		}

		public void SaveData()
		{
			WriteDataOnStorage();
		}

		private void WriteDataOnStorage()
		{
			using (var file = File.Create(SavePath))
			{
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(file, CachedData);
			}

			Debug.Log("Data saved");
		}

		public void RestoreData()
		{
			if (!HasSavedData)
			{
				Debug.Log("Has no saved data");
				return;
			}

			using (var file = File.Open(SavePath, FileMode.Open))
			{
				BinaryFormatter bf = new BinaryFormatter();
				CachedData = (SaveData)bf.Deserialize(file);
			}

			Debug.Log("Data restored");
		}

		public void DeleteData()
		{
			if (HasSavedData)
			{
				File.Delete(SavePath);
				CachedData = default(SaveData);
			}
		}
	}
}
