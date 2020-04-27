namespace Orbox.Utils
{
    public interface IPlayerPrefs
    {
        void DeleteAll();
        void DeleteKey(string key);
        bool HasKey(string key);

        int GetInt(string key, int defaultValue);
        float GetFloat(string key, float defaultValue);                
        string GetString(string key, string defaultValue);       

        void SetInt(string key, int value);
        void SetFloat(string key, float value);        
        void SetString(string key, string value);

        void Save();
    }
}