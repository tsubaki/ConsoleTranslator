using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManualTranslateLanguage : ScriptableObject
{
	
	public List<Param> list = new List<Param> ();
	
	[System.SerializableAttribute]
	public class Param
	{
		
		public string pattern;
		public string replaceFormat;
	}
}

