using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;

namespace TranslateLog
{
	public class TranslateLog : MonoBehaviour
	{
		public static string UpdateText (string log)
		{
			string[] msgs = log.Split ('\n');
			return UpdateBaseMessage (msgs [0]);
		}
	
		private static string UpdateBaseMessage (string str)
		{
			str = ExtractException (str);
			str = str.Replace (":", ".:");
			return str;
		}
	
		private static string ExtractException (string str)
		{
			Regex reg = new Regex ("(?<exception>^.*Exception)");
			Match m = reg.Match (str);
		
			if (!m.Success)
				return str;
		
			string exceptionName = m.Groups ["exception"].Value;
			string exceptioWord = ObjectNames.NicifyVariableName (exceptionName).Replace ("Exception", "");
			str = str.Replace (exceptionName, "error of '" + exceptioWord + "'");
		
			return str;
		}
	
		[MenuItem("Edit/translate/log %t")]
		public static void Method ()
		{
			string log = GetLogMessage.GetActiveLog ();
			string msg = UpdateText (log);

			// Try Manual Translate
			string tryReplace = ReplaceMessage.ManualTranslate (log);
			if (tryReplace != null) {
				TranslateWindow window = 
					EditorWindow.GetWindow (typeof(TranslateWindow)) as TranslateWindow;
				
				window.errorMessage = tryReplace;
				window.baseErrorMessage = msg;
				return;
			}


			// Try Online Translate
			MicrosoftTranslate translate = new MicrosoftTranslate ();
			try {
				translate.TranslateMethod (msg, (translated) =>
				{
					TranslateWindow window = 
			   			EditorWindow.GetWindow (typeof(TranslateWindow)) as TranslateWindow;

					window.errorMessage = translated;
					window.baseErrorMessage = msg;
				}); 
			} catch (WebException err) {
				Debug.LogError("translate failed. please check it 'id' and 'seclet key', or network. \n" + err.Message);
				TranslateSettings.ResetTranslateToken();
				EditorWindow.GetWindow<TranslateSettings> ();
			}
		}
	
		[MenuItem("Edit/translate/settings")]
		public static void Settings ()
		{
			EditorWindow.GetWindow<TranslateSettings> ();
		}
		public static string currentLanguage ()
		{
			
			TranslateLanguageType languageType = (TranslateLanguageType)EditorPrefs.GetInt ("translate_languageType");
			
			if (languageType == TranslateLanguageType.AUTO) {
				switch (Application.systemLanguage) {
				case SystemLanguage.Japanese:
					return "ja";
				case SystemLanguage.Korean:
					return "ko";
				case SystemLanguage.Chinese:
					return "zh-CHT";
				case SystemLanguage.Italian:
					return "it";
				default:
					return "en";
				}
			}
			
			switch (languageType) {
			case TranslateLanguageType.CHINA:
				return "zh-CHT";
			case TranslateLanguageType.ENGLISH:
				return "en";
			case TranslateLanguageType.ITALIA:
				return "it";
			case TranslateLanguageType.JAPANESE:
				return "ja";
			case TranslateLanguageType.KOREA:
				return "ko";
			default:
				return "en";
			}
		}
	}
}