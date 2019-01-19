using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ChangeVRMCloth : MonoBehaviour
{
    public string TextureName; 

    void ChangeCloth()
    {
        GameObject test = GameObject.FindGameObjectWithTag("VRM_Body");
        Material[] mat = test.GetComponent<SkinnedMeshRenderer>().materials;

        //ファイル読み込み
        var path = Application.streamingAssetsPath + "/" + TextureName + ".png";

        byte[] bytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2048,2048);
        texture.filterMode = FilterMode.Trilinear;
		texture.LoadImage(bytes);
        
        mat[3].SetTexture("_MainTex",texture);
        mat[3].SetTexture("_ShadeTexture",texture);
        
        test.GetComponent<SkinnedMeshRenderer>().materials = mat;
    }

    //オブジェクトが触れている間
    void OnCollisionEnter(Collision collision) {
        //if (Input.GetKeyDown(KeyCode.Return)) {  //Enterキーに調べるコマンドを割り当てる
            ChangeCloth();
        //}
    }
}
