using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace GS.PongFootball
{
    public class LanguageManager : MonoBehaviour
    {

        private Dictionary<string, Locale> availableLanguages = new Dictionary<string, Locale>();

        // ar, zh-Hans, en, fr, de, id, ja, ko, ru, es
        private Dictionary<int, string> languagesCode = new Dictionary<int, string> {
        {0,"en"}, {1,"ja"},{2,"zh-Hans"},{3,"ko"},{4,"ar"},{5,"fr"},{6,"es"},{7,"de"},{8,"ru"},{9,"id"}
        };

        private Dictionary<int, string> languagesCode_WebGL = new Dictionary<int, string> {
        {0,"en"},{1,"fr"},{2,"es"},{3,"de"},{4,"id"}
        };

        public int SelectedLanguage
        {
            get
            {
                return GameData.Instance.State.SelectedLanguage;
            }
            set
            {
                GameData.Instance.State.SelectedLanguage = value;
                LocaleSelected(value);
                GameData.Instance.Save();
            }
        }

        IEnumerator Start()
        {

            // Wait for the localization system to initialize
            yield return LocalizationSettings.InitializationOperation;

            // Generate list of available Locales
            //var options = new List<Dropdown.OptionData>();
            //int selected = 0;
            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
            {
                 var locale = LocalizationSettings.AvailableLocales.Locales[i];

                availableLanguages.Add(locale.Identifier.Code, locale);

                // if (LocalizationSettings.SelectedLocale == locale)
                //     selected = i;
                // options.Add(new Dropdown.OptionData(locale.name));
            }
            LocaleSelected(SelectedLanguage);
            
        }

        public void LocaleSelected(int index)
        {
            //var a = LocalizationSettings.SelectedLocale;
            //LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
            LocalizationSettings.SelectedLocale = availableLanguages[languagesCode_WebGL[index]];
            Debug.Log("OK");
        }

        public int TotalAvailableLanguage() => availableLanguages.Count;
        
    }
}