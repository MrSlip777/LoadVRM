using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.Cameras;
using VRM;

public class LoadVRM : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//"StreamingAssets"フォルダの"VRoid.vrm"を読み込む
		var path = Application.streamingAssetsPath + "/" + "VRoid.vrm";

		// Actionコールバックで生成されたGameObjectが返される
		VRMImporter.LoadVrmAsync(path, gameObject =>
		{
			//タグ：プレイヤーのオブジェクトを対象とする
			gameObject.transform.position = new Vector3(7.0f, 1, 5);
			gameObject.tag = "Player";

			//アニメーターを設定する
			RuntimeAnimatorController asset = (RuntimeAnimatorController)Instantiate(Resources.Load("Animator/ThirdPersonAnimatorController"));
			gameObject.GetComponent<Animator>().runtimeAnimatorController = asset;

			//コライダーなどの設定
			gameObject.AddComponent<Rigidbody>();
			gameObject.AddComponent<CapsuleCollider>();
			gameObject.GetComponent<CapsuleCollider>().height = 1.85f;
			gameObject.GetComponent<CapsuleCollider>().radius = 0.2f;
			gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0,0.85f,0);
			gameObject.AddComponent<AnnouncePlayerPosition>();
			gameObject.AddComponent<CheckCommand>();
			gameObject.AddComponent<ThirdPersonUserControl>();

			//カメラを設定
			GameObject camera = GameObject.Find("FreeLookCameraRig");
			camera.GetComponent<FreeLookCam>().target = gameObject.transform;
			
		});		
	}
}
