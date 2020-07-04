using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogwheel.Localization
{
    public class LocalizationSystem
    {
        public enum Language
        {
            English,
            Deutsch,
            French
        }

        public static Language language = Language.English;

        private static Dictionary<string, string> LocalisedEN;
        private static Dictionary<string, string> LocalisedDE;
        private static Dictionary<string, string> LocalisedFR;

        public static bool isInit;

        public static CSVLoader csvLoader;

        public static void Init()
        {
            csvLoader = new CSVLoader();
            csvLoader.LoadCSV();

            UpdateDictionaries();

            isInit = true;
        }

        public static void UpdateDictionaries()
        {
            LocalisedEN = csvLoader.GetDictionaryValues("en");
            LocalisedDE = csvLoader.GetDictionaryValues("de");
            LocalisedFR = csvLoader.GetDictionaryValues("fr");
        }

        public static Dictionary<string, string> GetDictionaryForEditor()
        {
            if (!isInit) { Init(); }
            return LocalisedEN;
        }

        public static string GetLocalisedValue(string key)
        {
            if (!isInit) { Init(); }

            string value = key;

            switch(language)
            {
                case Language.English:
                    LocalisedEN.TryGetValue(key, out value);
                    break;
                case Language.Deutsch:
                    LocalisedDE.TryGetValue(key, out value);
                    break;
                case Language.French:
                    LocalisedFR.TryGetValue(key, out value);
                    break;
            }

            return value;
        }

#if UNITY_EDITOR
        public static void Add(string key, string value)
        {
            if(value.Contains("\""))
            {
                value.Replace('"', '\"');
            }

            if(csvLoader == null)
            {
                csvLoader = new CSVLoader();
            }

            csvLoader.LoadCSV();
            csvLoader.Add(key, value);
            csvLoader.LoadCSV();

            UpdateDictionaries();
        }

        public static void Replace(string key, string value)
        {
            if (value.Contains("\""))
            {
                value.Replace('"', '\"');
            }

            if (csvLoader == null)
            {
                csvLoader = new CSVLoader();
            }

            csvLoader.LoadCSV();
            csvLoader.Edit(key, value);
            csvLoader.LoadCSV();

            UpdateDictionaries();
        }

        public static void Remove(string key)
        {
            if (csvLoader == null)
            {
                csvLoader = new CSVLoader();
            }

            csvLoader.LoadCSV();
            csvLoader.Remove(key);
            csvLoader.LoadCSV();

            UpdateDictionaries();
        }
#endif
    }
}
