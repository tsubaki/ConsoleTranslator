using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;
using System;


namespace TranslateLog
{
	public class GetLogMessage
	{
		public static string GetActiveLog ()
		{
			try {
				var assembly = Assembly.Load ("UnityEditor.dll");
				var typeOfConsoleWindow = assembly.GetType ("UnityEditor.ConsoleWindow");
				var instanceOfConsole = typeOfConsoleWindow.GetField ("ms_ConsoleWindow",
                                                              BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue (null);
				return (string)typeOfConsoleWindow.GetField ("m_ActiveText",
                                                         BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue (instanceOfConsole);
			} catch (TargetException error) {
				Debug.Log (error.Message);
				return "please open the console. (window>console)";
			} catch (Exception err) {
				return err.Message;
			}
		}
	}
}