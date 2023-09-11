using System.Collections.Generic;
using UnityEngine;

namespace CarmineNeo.Misc.Localisation {
    public class LocalisationSystem {
        public static Language language = Language.en_US;

        private static Dictionary<string, string> localisedEN;
        private static Dictionary<string, string> localisedFR;

        public static bool isInit;

        public static void Init() {
            CSVLoader csvLoader = new CSVLoader();
            csvLoader.LoadCSV();

            localisedEN = csvLoader.GetDictionaryValues("en_US");
            localisedFR = csvLoader.GetDictionaryValues("fr_CA");

            isInit = true;
        }

        public static string GetLocalisedValue(string key) {
            if(!isInit)
                Init();

            string value = key;

            switch(language) {
                case Language.en_US:
                    localisedEN.TryGetValue(key, out value);
                    break;
                case Language.fr_CA:
                    localisedFR.TryGetValue(key, out value);
                    break;
            }

            return value;
        }

        public enum Language {
            de_DE,
            en_US,
            fr_CA,
            nl_NL,
            pt_BR,
            ru_RU
        }
    }
}