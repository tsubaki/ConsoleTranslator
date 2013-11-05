using UnityEngine;
using System.Collections;
using UnityEditor;

namespace TranslateLog
{
	public class TranslateSettings :  EditorWindow
	{
		string id, secret;
		TranslateLanguageType languageType;

		string preId, preSecret;
		TranslateLanguageType preLanguageType;

		TranslateSettings ()
		{
			id = EditorPrefs.GetString ("translate_clientId");
			secret = EditorPrefs.GetString ("translate_clientSecret");
			preId = id;
			preSecret = secret;
			languageType = (TranslateLanguageType)EditorPrefs.GetInt ("translate_languageType", 0);
			preLanguageType = languageType;
		}

		void OnGUI ()
		{
			EditorGUI.BeginChangeCheck ();
			{
				id = EditorGUILayout.TextField ("id", id);
				secret = EditorGUILayout.TextField ("secret", secret);
				languageType = (TranslateLanguageType)EditorGUILayout.EnumPopup ("language", languageType);
			}
			if (EditorGUI.EndChangeCheck ()) {
				ResetTranslateToken();
				UpdateSettings(id, secret, (int)languageType);
			}

			GUILayout.BeginHorizontal();
			if (GUILayout.Button ("OK")) {
				Close ();
			}
			if (GUILayout.Button ("cancel")) {
				UpdateSettings(preId, preSecret, (int)preLanguageType);
				Close ();
			}
			GUILayout.EndHorizontal();
		}

		public static void ResetTranslateToken()
		{
			EditorPrefs.DeleteKey ("translate_date");
			EditorPrefs.DeleteKey ("translate_token");
		}
		public void UpdateSettings(string id, string secret, int languageType)
		{
			EditorPrefs.SetString ("translate_clientId", id);
			EditorPrefs.SetString ("translate_clientSecret", secret);
			EditorPrefs.SetInt ("translate_languageType", languageType);
		}
	}
}