namespace Orbox.Utils
{
    public class PlayerPrefsMockup : IPlayerPrefs
    {
        public void DeleteAll()
        {            
        }

        public void DeleteKey(string key)
        {            
        }

        public float GetFloat(string key, float defaultValue)
        {
            return defaultValue;
        }

        public int GetInt(string key, int defaultValue)
        {
            return defaultValue;
        }

        public string GetString(string key, string defaultValue)
        {
            return defaultValue;
        }

        public bool HasKey(string key)
        {
            return false;
        }

        public void Save()
        {            
        }

        public void SetFloat(string key, float value)
        {            
        }

        public void SetInt(string key, int value)
        {            
        }

        public void SetString(string key, string value)
        {            
        }
    }
}