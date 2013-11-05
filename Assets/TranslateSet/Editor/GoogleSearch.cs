using UnityEngine;
using System.Collections;

namespace TranslateLog
{
	public class GoogleSearch
	{
 
		//static string template = "http://www.google.com/cse?url=http%3A%2F%2Fforum.unity3d.com%2Fforum.php&cref=https%3A%2F%2Fwww.google.com%2Fcse%2Ftools%2Fmakecse%3Furl%3Dhttp%253A%252F%252Fforum.unity3d.com%252Fforum.php&ie=&q=$parameter$&sa=%E6%A4%9C%E7%B4%A2#gsc.tab=0&gsc.q=$parameter$&gsc.page=1";
		//static string template = "http://search.unity3d.com/uss1/?q=$parameter$&app=All";
		static string template = "https://www.google.co.jp/search?q=$parameter$+site%3Ahttp%3A%2F%2Fforum.unity3d.com";
    
		public static string CreateSerachURLInDoc (string msg)
		{
			msg = replaceSearchMessage (msg);
			string url = WWW.EscapeURL (msg);
			return template.Replace ("$parameter$", url);
		}

		static string replaceSearchMessage (string msg)
		{
			// remove exception name.
			int count = msg.LastIndexOf (":");
			msg = msg.Substring (count + 1, msg.Length - count - 1);

			// remove " & '  
			msg = msg.Replace ("\"", "");
			msg = msg.Replace ("'", "");
			msg = msg.Replace ("`", "");
			return msg;
		}
	}
}