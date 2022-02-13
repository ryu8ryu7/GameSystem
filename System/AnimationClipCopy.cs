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
		// AnimationClip������FBX�̃p�X
		string fbxPath = "Assets/UnityChan/SD_unitychan/Animations/SD_unitychan_motion_Generic.fbx";
		// AnimationClip�̃t�@�C����
		string clipName = "GoDown";
		// AnimationClip�̏o�͐�
		string exportPath = "Assets/Clone.anim";

		string tempExportedClip = "Assets/tempClip.anim";

		// AnimationClip�̎擾
		var animations = AssetDatabase.LoadAllAssetsAtPath(fbxPath);
		var originalClip = System.Array.Find<Object>(animations, item =>
			  item is AnimationClip && item.name == clipName
		);

		// AnimationClip���R�s�[���ďo��(���j�[�N��uuid)
		var copyClip = Object.Instantiate(originalClip);
		AssetDatabase.CreateAsset(copyClip, tempExportedClip);

		// AnimationClip�̃R�s�[�i�Œ艻����uuid�j
		File.Copy(tempExportedClip, exportPath, true);
		File.Delete(tempExportedClip);

		// AssetDatabase���t���b�V��
		AssetDatabase.Refresh();
	}
}
