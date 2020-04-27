using UnityEngine;

namespace Orbox.Localization
{
    public interface ILanguageProvider
    {
        SystemLanguage GetLanguage();
    }

}
