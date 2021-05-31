/*
 * MToonシェーダー　→　URP Litシェーダーへ置換するUnityエディタ拡張スクリプト
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

using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace VRM
{
    public class Change_MToonToURPToonLit : ScriptableWizard
    {
        private static GameObject m_Wizard;

        //対象になるモデル
        public GameObject targetModel;
        private readonly string OutputFolderName = "Assets/Models/Resources";

        private readonly string TextureFolderName = "Texture";
        private readonly string MaterialFolderName = "Material";

        private readonly string ShaderName_MToon = "VRM/MToon";
        private readonly string ShaderName_Target = "SimpleURPToonLitExample(With Outline)";

        public static void CreateWizard()
        {
            var wiz = ScriptableWizard.DisplayWizard<Change_MToonToURPToonLit>(
                "Change_MToonToURPToonLit", "change");
            var go = Selection.activeObject as GameObject;
            m_Wizard = new GameObject();

        }

        void OnWizardCreate()
        {
            SkinnedMeshRenderer[] skinnedMeshRenderers
             = targetModel.GetComponentsInChildren<SkinnedMeshRenderer>();

            //変更後のシェーダーを設定
            Shader changeShader = Shader.Find(ShaderName_Target);

            foreach(SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers){

                Material[] materials = skinnedMeshRenderer.sharedMaterials;

                for(int i = 0; i<materials.Length; i++){
                    Material material = materials[i];

                    if(material.shader.name == ShaderName_MToon){

                        Texture input_Tex = material.mainTexture;

                        //フォルダ作成
                        MakeFolderForShaders();

                        //マテリアルの設定反映
                        SettingProperty_FromMtoonToHDRP(ref material,input_Tex,changeShader);

                    }
                }
            }
        }

        void OnDestroy(){
            DestroyImmediate(m_Wizard);
        }

        void MakeFolderForShaders(){
            string shaderFolderName = null;

            shaderFolderName = ShaderName_MToon.Replace('/', '_');

            MakeFolder_OutputFiles(shaderFolderName);

            shaderFolderName = ShaderName_Target.Replace('/', '_');

            MakeFolder_OutputFiles(shaderFolderName);

        }

        void MakeFolder_OutputFiles(string shaderFolderName){
            string targetFolderPath = null;

            targetFolderPath = OutputFolderName + "/" + targetModel.name + "/" 
                + MaterialFolderName + "/" + shaderFolderName;

            if (!Directory.Exists(targetFolderPath)) {
                Directory.CreateDirectory(targetFolderPath);
            }

            targetFolderPath = OutputFolderName + "/" + targetModel.name + "/" 
                + TextureFolderName + "/" + shaderFolderName;

            if (!Directory.Exists(targetFolderPath)) {
                Directory.CreateDirectory(targetFolderPath);
            }
        }

        void SettingProperty_FromMtoonToHDRP(ref Material material,Texture input_Tex,Shader changeShader){
            string shaderFolderName = null;

            Texture mainTex = material.GetTexture("_MainTex");
            Texture normalMap = material.GetTexture("_BumpMap");
            Color mainColor = material.GetColor("_Color");

            material.shader = changeShader;

            material.SetTexture("_BaseColorMap",mainTex);
            material.SetFloat("_AlphaCutoffEnable",1.0f);
            material.SetTexture("_EmissiveColorMap",mainTex);
            material.SetTexture("_NormalMap",normalMap);
            material.SetColor("_BaseColor",mainColor);
            material.SetFloat("_Metallic",0.0f);
            material.SetFloat("_Smoothness",0.0f);

        }

    }

    public static class Change_MToonToHDRP_Menu
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/MaterialSetting/Change_MToonToHDRP";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void Menu()
        {
           Change_MToonToHDRP.CreateWizard();
        }
    }
}
