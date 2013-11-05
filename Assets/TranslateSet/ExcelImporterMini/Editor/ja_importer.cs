using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ja_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/TranslateSet/Editor/language/ja.xls";
	private static readonly string exportPath = "Assets/TranslateSet/Editor/language/ja.asset";
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			ManualTranslateLanguage data = (ManualTranslateLanguage)AssetDatabase.LoadAssetAtPath (exportPath, typeof(ManualTranslateLanguage));
			if (data == null) {
				data = ScriptableObject.CreateInstance<ManualTranslateLanguage> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.list.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read)) {
				IWorkbook book = new HSSFWorkbook (stream);
				ISheet sheet = book.GetSheetAt (0);
				
				for (int i=1; i<= sheet.LastRowNum; i++) {
					IRow row = sheet.GetRow (i);
					ICell cell = null;
					
					ManualTranslateLanguage.Param p = new ManualTranslateLanguage.Param ();
					
					cell = row.GetCell(0); p.pattern = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(1); p.replaceFormat = (cell == null ? "" : cell.StringCellValue);
					data.list.Add (p);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
