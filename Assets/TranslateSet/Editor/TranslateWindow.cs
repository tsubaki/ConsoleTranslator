using UnityEngine;
using System.Collections;
using UnityEditor;

namespace TranslateLog
{
	public class TranslateWindow : EditorWindow
	{
 
		public string errorMessage;
		public string baseErrorMessage;
 
		void OnGUI ()
		{
			GUILayout.BeginHorizontal ();
				
			GUI.BeginGroup (new Rect (5, 0, position.width - 10, position.height - 25));
			GUILayout.TextArea (errorMessage, GUILayout.Height (position.height), GUILayout.Width (position.width - 20));
			GUI.EndGroup ();
				
			if (GUI.Button (new Rect (position.width - 215, position.height - 25, 200, 22), "ask unity community")) {
				string url = GoogleSearch.CreateSerachURLInDoc (baseErrorMessage);
				Application.OpenURL (url);
			}
				
			GUILayout.EndHorizontal ();
		}
	}

}