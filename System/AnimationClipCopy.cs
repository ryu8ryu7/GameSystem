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
			.Where(x => x.EndsWith(".FBX"))
			.Distinct()
			.OrderBy(x => x)
			.ToArray()
			;


		return;
		// AnimationClipを持つFBXのパス
		string fbxPath = "Assets/UnityChan/SD_unitychan/Animations/SD_unitychan_motion_Generic.fbx";
		// AnimationClipのファイル名
		string clipName = "GoDown";
		// AnimationClipの出力先
		string exportPath = "Assets/Clone.anim";

		string tempExportedClip = "Assets/tempClip.anim";

		// AnimationClipの取得
		var animations = AssetDatabase.LoadAllAssetsAtPath(fbxPath);
		var originalClip = System.Array.Find<Object>(animations, item =>
			  item is AnimationClip && item.name == clipName
		);

		// AnimationClipをコピーして出力(ユニークなuuid)
		var copyClip = Object.Instantiate(originalClip);
		AssetDatabase.CreateAsset(copyClip, tempExportedClip);

		// AnimationClipのコピー（固定化したuuid）
		File.Copy(tempExportedClip, exportPath, true);
		File.Delete(tempExportedClip);

		// AssetDatabaseリフレッシュ
		AssetDatabase.Refresh();
	}
}
