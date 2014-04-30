using UnityEngine;
using System.Collections;

public class Hoge : MonoBehaviour
{
	
	AsyncOperation hoge;
	void OnGUI ()
	{
		if (GUILayout.Button ("hoge")) {
			hoge = Application.LoadLevelAdditiveAsync (1);
		}
		
		if (GUILayout.Button ("hoge")) {
			hoge.allowSceneActivation = true;
		}
	}
}
