﻿using UnityEngine;
using System.Collections;

public class LanguageHelper
{

    /// <summary>
    /// Helps to convert Unity's Application.systemLanguage to a 
    /// 2 letter ISO country code. There is unfortunately not more
    /// countries available as Unity's enum does not enclose all
    /// countries. See https://github.com/MartinSchultz/unity3d (Martin 
    /// Schultz)
    /// </summary>
    /// <returns>The 2-letter ISO code from system language.</returns>
    public static string Get2LetterISOCodeFromSystemLanguage()
    {
        SystemLanguage lang = Application.systemLanguage;
        string res = "en";
        switch (lang)
        {
            case SystemLanguage.Afrikaans: res = "af"; break;
            case SystemLanguage.Arabic: res = "ar"; break;
            case SystemLanguage.Basque: res = "eu"; break;
            case SystemLanguage.Belarusian: res = "by"; break;
            case SystemLanguage.Bulgarian: res = "bg"; break;
            case SystemLanguage.Catalan: res = "ca"; break;
            case SystemLanguage.Chinese: res = "zh"; break;
            case SystemLanguage.Czech: res = "cs"; break;
            case SystemLanguage.Danish: res = "da"; break;
            case SystemLanguage.Dutch: res = "nl"; break;
            case SystemLanguage.English: res = "en"; break;
            case SystemLanguage.Estonian: res = "et"; break;
            case SystemLanguage.Faroese: res = "fo"; break;
            case SystemLanguage.Finnish: res = "fi"; break;
            case SystemLanguage.French: res = "fr"; break;
            case SystemLanguage.German: res = "de"; break;
            case SystemLanguage.Greek: res = "el"; break;
            case SystemLanguage.Hebrew: res = "iw"; break;
            case SystemLanguage.Hungarian: res = "hu"; break;
            case SystemLanguage.Icelandic: res = "is"; break;
            case SystemLanguage.Indonesian: res = "in"; break;
            case SystemLanguage.Italian: res = "it"; break;
            case SystemLanguage.Japanese: res = "ja"; break;
            case SystemLanguage.Korean: res = "ko"; break;
            case SystemLanguage.Latvian: res = "lv"; break;
            case SystemLanguage.Lithuanian: res = "lt"; break;
            case SystemLanguage.Norwegian: res = "no"; break;
            case SystemLanguage.Polish: res = "pl"; break;
            case SystemLanguage.Portuguese: res = "pt"; break;
            case SystemLanguage.Romanian: res = "ro"; break;
            case SystemLanguage.Russian: res = "ru"; break;
            case SystemLanguage.SerboCroatian: res = "sh"; break;
            case SystemLanguage.Slovak: res = "sk"; break;
            case SystemLanguage.Slovenian: res = "sl"; break;
            case SystemLanguage.Spanish: res = "es"; break;
            case SystemLanguage.Swedish: res = "sv"; break;
            case SystemLanguage.Thai: res = "th"; break;
            case SystemLanguage.Turkish: res = "tr"; break;
            case SystemLanguage.Ukrainian: res = "uk"; break;
            case SystemLanguage.Unknown: res = "en"; break;
            case SystemLanguage.Vietnamese: res = "vi"; break;
        }
        //		Debug.Log ("Lang: " + res);
        return res;
    }
}