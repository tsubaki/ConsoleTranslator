using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace TranslateLog
{
	public class ReplaceMessage
	{

		public static string ManualTranslate (string input)
		{
			ManualTranslateLanguage language = SerachAsset ();
			if (language == null) {
				return null;
			}

			foreach (var item in language.list) {
				var match = Regex.Match (input, item.pattern);
				if( match.Success){
					return Replace(match, item.replaceFormat);
				}
			}
			return null;
		}

		static ManualTranslateLanguage SerachAsset()
		{
			string languageType = TranslateLog.currentLanguage ();
			
			ManualTranslateLanguage language = null;
			var filePattern = string.Format ("{0}.asset", languageType);
			
			var files = Directory.GetFiles (Application.dataPath, filePattern, SearchOption.AllDirectories);
			foreach (string path in files) {
				string newPath = path.Replace (Application.dataPath, "Assets");
				language = (ManualTranslateLanguage)AssetDatabase.LoadAssetAtPath (newPath, typeof(ManualTranslateLanguage));
				break;
			}
			return language;
		}

		public static string Replace (Match match, string replaceFormat)
		{
			List<string> list = new List<string> ();
			for (int i=0; i< match.Groups.Count; i++) {
				list.Add (match.Groups [i].Value);
			}

			return string.Format (replaceFormat, list.ToArray ());
		}
	}
}

