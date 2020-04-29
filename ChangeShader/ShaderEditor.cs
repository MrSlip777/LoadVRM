/*
 * 対象のモデルのシェーダーを変更するスクリプト
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
    static ShaderEditor m_ChangeShader = new ShaderEditor();

    [MenuItem("ShaderEditor/Change")]
    private static void ApplySettingMenu()
    {
        EditorWindow.GetWindow<ChangeShaderMenu>( "ChangeShader" );
    }

    readonly float UIdistance = 30.0f;
    readonly float TitlePos_Y = 50.0f;

    private void OnGUI()
    {
        // ラベル表示
        EditorGUILayout.LabelField( "対象オブジェクトを選択してください（親オブジェクト）" );
        targetModel = EditorGUILayout.ObjectField(targetModel, typeof(GameObject), true);

        EditorGUI.LabelField( new Rect(0.0f, TitlePos_Y, position.width, 20),"ArkToon" );

        if( GUI.Button ( new Rect( 10.0f, TitlePos_Y + UIdistance, 120.0f, 20.0f), "Opaque") ){
			m_ChangeShader.ChangeShader((GameObject)targetModel,"arktoon/Opaque");
		}
        if( GUI.Button ( new Rect( 10.0f, TitlePos_Y + UIdistance*2, 120.0f, 20.0f), "AlphaCutout") ){
			m_ChangeShader.ChangeShader((GameObject)targetModel,"arktoon/AlphaCutout");
		}
        
        if( GUI.Button ( new Rect( 10.0f, TitlePos_Y + UIdistance*3, 120.0f, 20.0f), "Fade") ){
			m_ChangeShader.ChangeShader((GameObject)targetModel,"arktoon/Fade");
		}
        if( GUI.Button ( new Rect( 10.0f, TitlePos_Y + UIdistance*4, 120.0f, 20.0f), "FadeRefracted") ){
			m_ChangeShader.ChangeShader((GameObject)targetModel,"arktoon/FadeRefracted");
		}

        float Title2_Pos_Y = TitlePos_Y + UIdistance*5;

        EditorGUI.LabelField( new Rect(0.0f, Title2_Pos_Y, position.width, 20),"MToon" );

        if( GUI.Button ( new Rect( 10.0f, Title2_Pos_Y + UIdistance, 120.0f, 20.0f), "MToon") ){
			m_ChangeShader.ChangeShader((GameObject)targetModel,"VRM/MToon");
		}

        float Title3_Pos_Y = Title2_Pos_Y + UIdistance*2;

        EditorGUI.LabelField( new Rect(0.0f, Title3_Pos_Y, position.width, 20),"UTS2" );

        if( GUI.Button ( new Rect( 10.0f, Title3_Pos_Y + UIdistance, 120.0f, 20.0f), "Toon_DoubleShadeWithFeather") ){
			m_ChangeShader.ChangeShader((GameObject)targetModel,"UnityChanToonShader/Toon_DoubleShadeWithFeather");
		}
        if( GUI.Button ( new Rect( 10.0f, Title3_Pos_Y + UIdistance*2, 120.0f, 20.0f), "ToonColor_DoubleShadeWithFeather_Clipping_StencilMask") ){
			m_ChangeShader.ChangeShader((GameObject)targetModel,"UnityChanToonShader/NoOutline/ToonColor_DoubleShadeWithFeather_Clipping_StencilMask");
		}
    }
}

public class ShaderEditor : MonoBehaviour {

    public void ChangeShader(GameObject targetModel,string shaderName)
    {
        Shader shader1 = Shader.Find(shaderName);
        SkinnedMeshRenderer[] skinnedMeshRenderers
            = targetModel.GetComponentsInChildren<SkinnedMeshRenderer>();

        for(int j = 0; j<skinnedMeshRenderers.Length; j++){
            Material[] materials = skinnedMeshRenderers[j].sharedMaterials;

            for(int i = 0; i<materials.Length; i++){
                materials[i].shader = shader1;

                if(shaderName.Contains("arktoon")){
                    materials[i].SetColor("_RimColor", Color.white);
                    materials[i].SetFloat("_Shadowborder", 0);
                    materials[i].SetFloat("_ShadowStrength", 0);
                }
                else{
                    materials[i].SetColor("_RimColor", Color.black);
                }
            }
        }
    }         
}

#endif