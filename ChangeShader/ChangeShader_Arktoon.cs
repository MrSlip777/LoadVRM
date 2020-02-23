/*
 * 対象のモデルのシェーダーをArktoonにするスクリプト
 *
 * (C)2020 slip
 * This software is released under the MIT License.
 * http://opensource.org/licenses/mit-license.php
 * [Twitter]: https://twitter.com/kjmch2s/
 *
 * 利用規約：
 *  作者に無断で改変、再配布が可能で、利用形態（商用、18禁利用等）
 *  についても制限はありません。
 *  このスクリプトはもうあなたのものです。
 * 
 */
#if UNITY_EDITOR

using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;

public class ChangeShaderMenu : EditorWindow{
    public Object targetModel;
    static ChangeShader_Arktoon m_ChangeShader_Arktoon = new ChangeShader_Arktoon();

    [MenuItem("ShaderEditor/Change/Arktoon")]
    private static void ApplySettingMenu()
    {
        EditorWindow.GetWindow<ChangeShaderMenu>( "Change_Arktoon" );
    }

    private void OnGUI()
    {
        // 試しにラベルを表示
        EditorGUILayout.LabelField( "対象オブジェクトを選択してください（親オブジェクト）" );
        targetModel = EditorGUILayout.ObjectField(targetModel, typeof(GameObject), true);

        if( GUI.Button ( new Rect( 0.0f, 40.0f, 120.0f, 20.0f), "Opaque") ){
			m_ChangeShader_Arktoon.ChangeShader((GameObject)targetModel,"arktoon/Opaque");
		}
        if( GUI.Button ( new Rect( 0.0f, 70.0f, 120.0f, 20.0f), "AlphaCutout") ){
			m_ChangeShader_Arktoon.ChangeShader((GameObject)targetModel,"arktoon/AlphaCutout");
		}
        if( GUI.Button ( new Rect( 0.0f, 100.0f, 120.0f, 20.0f), "Fade") ){
			m_ChangeShader_Arktoon.ChangeShader((GameObject)targetModel,"arktoon/Fade");
		}
        if( GUI.Button ( new Rect( 0.0f, 130.0f, 120.0f, 20.0f), "FadeRefracted") ){
			m_ChangeShader_Arktoon.ChangeShader((GameObject)targetModel,"arktoon/FadeRefracted");
		}        
    }
}

public class ChangeShader_Arktoon : MonoBehaviour {

    public void ChangeShader(GameObject targetModel,string shaderName)
    {
        Shader shader1 = Shader.Find(shaderName);
        SkinnedMeshRenderer[] skinnedMeshRenderers
            = targetModel.GetComponentsInChildren<SkinnedMeshRenderer>();

        for(int j = 0; j<skinnedMeshRenderers.Length; j++){
            Material[] materials = skinnedMeshRenderers[j].sharedMaterials;

            for(int i = 0; i<materials.Length; i++){
                materials[i].shader = shader1;
            }
        }
    }         
}

#endif