/*
 * HDRP Litシェーダー →　MToonシェーダー　へ置換するUnityエディタ拡張スクリプト
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
    public class Change_HDRPToMToon : ScriptableWizard
    {
        private static GameObject m_Wizard;

        //対象になるモデル
        public GameObject targetModel;
        private readonly string OutputFolderName = "Assets/Models/Resources";

        private readonly string TextureFolderName = "Texture";
        private readonly string MaterialFolderName = "Material";

        private readonly string ShaderName_MToon = "VRM/MToon";
        private readonly string ShaderName_HDRP = "HDRP/Lit";

        public static void CreateWizard()
        {
            var wiz = ScriptableWizard.DisplayWizard<Change_HDRPToMToon>(
                "Change_MToonToHDRP", "change");
            var go = Selection.activeObject as GameObject;
            m_Wizard = new GameObject();

        }

        void OnWizardCreate()
        {
            SkinnedMeshRenderer[] skinnedMeshRenderers
             = targetModel.GetComponentsInChildren<SkinnedMeshRenderer>();

            //変更後のシェーダーを設定
            Shader changeShader = Shader.Find(ShaderName_MToon);

            foreach(SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers){

                Material[] materials = skinnedMeshRenderer.sharedMaterials;

                for(int i = 0; i<materials.Length; i++){
                    Material material = materials[i];

                    if(material.shader.name == ShaderName_HDRP){

                        Texture input_Tex = material.mainTexture;

                        //フォルダ作成
                        MakeFolderForShaders();

                        //マテリアルの設定反映
                        SettingProperty_FromHDRPToMToon(ref material,input_Tex,changeShader);

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

            shaderFolderName = ShaderName_HDRP.Replace('/', '_');

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

        void SettingProperty_FromHDRPToMToon(ref Material material,Texture input_Tex,Shader changeShader){
            string shaderFolderName = null;

            Texture mainTex = material.GetTexture("_BaseColorMap");
            Texture normalMap = material.GetTexture("_NormalMap");
            Color mainColor = material.GetColor("_BaseColor");

            material.shader = changeShader;

            material.SetTexture("_MainTex",mainTex);
            material.SetTexture("_ShadeTexture",mainTex);
            material.SetTexture("_BumpMap",normalMap);
            material.SetColor("_Color",mainColor);

        }

    }

    public static class Change_HDRPToMToon_Menu
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/MaterialSetting/Change_HDRPToMToon";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void Menu()
        {
           Change_HDRPToMToon.CreateWizard();
        }
    }
}
