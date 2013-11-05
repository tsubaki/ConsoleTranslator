using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic; 
using System.Xml;
using System;

namespace TranslateLog
{
	public  enum TranslateLanguageType
	{
		AUTO,
		JAPANESE,
		ENGLISH,
		KOREA,
		CHINA,
		ITALIA,
	};

	public class MicrosoftTranslate
	{
		public readonly static string DatamarketAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
		public delegate void TranslateResultMessage (string msg);

		public void TranslateMethod (string text, TranslateResultMessage translateResult)
		{
			string token = GetToken ();
        
			string from = "en";
			string to = TranslateLog.currentLanguage();
        
			if (to == "en") {
				translateResult (text);
				return;
			}
        
			string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text="
				+ WWW.EscapeURL (text) + "&from=" + from + "&to=" + to;
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create (uri);
			httpWebRequest.Headers.Add ("Authorization", token);
        
			WebResponse response = null;
			try {
				response = httpWebRequest.GetResponse ();
				string xml = string.Empty;
				using (StreamReader stream = new StreamReader(response.GetResponseStream())) {
					xml = stream.ReadToEnd (); 
				}
            
				XmlDocument doc = new XmlDocument ();
				doc.LoadXml (xml);
            
				translateResult (doc.InnerText);
			} finally { 
				if (response != null) {
					response.Close ();
					response = null;
				}
			} 
		}

		public string GetToken ()
		{
			string clientId = EditorPrefs.GetString ("translate_clientId", "unity_msg_translate");
			string clientSecret = EditorPrefs.GetString ("translate_clientSecret", "b0AWFfeMIS6NpR95wI0bnirRbSuOxX7l74SfJf");
		
			if ( string.IsNullOrEmpty( clientId ) || string.IsNullOrEmpty( clientSecret )) {
				throw new Exception ("set up transrater Editor>translate>settings.");
			}

			if (EditorPrefs.HasKey ("translate_date") && (DateTime.Now - DateTime.Parse (EditorPrefs.GetString ("translate_date"))).Minutes < 8) {
				return EditorPrefs.GetString ("translate_token");
			}
			string request = string.Format ("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com",
                                           WWW.EscapeURL (clientId), 
                                           WWW.EscapeURL (clientSecret));
			WebRequest webRequest = WebRequest.Create (DatamarketAccessUri);
			webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.Method = "POST";
			byte[] bytes = Encoding.ASCII.GetBytes (request);
			webRequest.ContentLength = bytes.Length;
			Dictionary<string,object> dic = null;

			using (Stream outputStream = webRequest.GetRequestStream()) {
				outputStream.Write (bytes, 0, bytes.Length);
			}
            
			using (WebResponse webResponse = webRequest.GetResponse()) {
				StreamReader r = new StreamReader (webResponse.GetResponseStream ());
                
				string json = r.ReadToEnd ();
				dic = MiniJSON.Json.Deserialize (json) as Dictionary<string,object>;
			}

			string authToken = "Bearer " + (string)dic ["access_token"];
		
			EditorPrefs.SetString ("translate_token", authToken);
			EditorPrefs.SetString ("translate_date", DateTime.Now.ToString ());
		
			return authToken;
		}
    

	}


}