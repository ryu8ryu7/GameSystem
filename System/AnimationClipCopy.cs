using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class Copy
{
	[MenuItem("Assets/AnimationClipCopy")]
	private static void AnimationClipCopy()
	{
		var list = Selection
			.GetFiltered<UnityEngine.Object>(SelectionMode.Assets | SelectionMode.DeepAssets)
			.Select(x => AssetDatabase.GetAssetPath(x))
			.SelectMany(x => AssetDatabase.GetDependencies(x))
			.Where(x => x.EndsWith(".FBX") || x.EndsWith(".fbx") )
			.Distinct()
			.OrderBy(x => x)
			.ToArray()
			;

		for (int i = 0; i < list.Length; i++)
		{
			AnimationClipCopy(list[i]);
		}
	}

	private static void AnimationClipCopy( string fbxPath )
	{ 
		// AnimationClipの出力先
		string exportPath = "";

		string tempExportedClip = "Assets/tempClip.anim";

		// AnimationClipの取得
		var animations = AssetDatabase.LoadAllAssetsAtPath(fbxPath);
        var originalClipArray = System.Array.FindAll<Object>(animations, item =>
              item is AnimationClip && !item.name.StartsWith("__preview")
        );

		string basePath = System.IO.Path.GetFileNameWithoutExtension(fbxPath);
		string folderPath = System.IO.Path.GetDirectoryName(fbxPath);

		foreach (var originalClip in originalClipArray)
		{
			exportPath = folderPath + "/";
			string fileName = basePath;
			if( originalClip.name != "Take 001" )
            {
				fileName += "_" + originalClip.name;
            }
			fileName += ".anim";

			exportPath += fileName;
			tempExportedClip = "Assets/" + fileName;

			// AnimationClipをコピーして出力(ユニークなuuid)
			var copyClip = Object.Instantiate(originalClip);
            AssetDatabase.CreateAsset(copyClip, tempExportedClip);

            // AnimationClipのコピー（固定化したuuid）
            File.Copy(tempExportedClip, exportPath, true);
			File.Delete(tempExportedClip);

			AssetDatabase.ImportAsset(exportPath);
		}
	}
}
