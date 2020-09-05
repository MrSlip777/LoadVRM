/*
 * UTS2シェーダー →　MToonシェーダーへ置換するUnityエディタ拡張スクリプト
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
    public class Change_UTS2ToMToon : ScriptableWizard
    {
        private static GameObject m_Wizard;

        //対象になるモデル
        public GameObject targetModel;
        private readonly string OutputFolderName = "Assets/Models/Resources";

        private readonly string TextureFolderName = "Texture";
        private readonly string MaterialFolderName = "Material";

        private readonly string ShaderName_MToon = "VRM/MToon";
        private readonly string ShaderName_UTS2 = "UnityChanToonShader/Toon_DoubleShadeWithFeather_Clipping";

        public static void CreateWizard()
        {
            var wiz = ScriptableWizard.DisplayWizard<Change_UTS2ToMToon>(
                "Change_UTS2ToMToon", "change");
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

                    if(material.shader.name == ShaderName_UTS2){

                        Texture input_Tex = material.mainTexture;
                        Texture input_Mask = material.GetTexture("_ClippingMask");

                        //フォルダ作成
                        MakeFolderForShaders();

                        //MToonのMaterial書き出し(バックアップ用)
                        SaveMaterial(material,ShaderName_UTS2);

                        //UTS2用にマスクテクスチャを作成
                        MakeMaskTexture(input_Tex,input_Mask);

                        //マテリアルの設定反映
                        SettingProperty_FromUTS2ToMToon(ref material,input_Tex,changeShader);

                        //UTS2のMaterial書き出し
                        SaveMaterial(material,ShaderName_MToon);

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

            shaderFolderName = ShaderName_UTS2.Replace('/', '_');

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

        void TransferTextureToTexture2D(ref Texture2D texture,Texture input_Tex){
            RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, 32);

            // もとのテクスチャをRenderTextureにコピー
            Graphics.Blit(input_Tex, renderTexture);
            RenderTexture.active = renderTexture;
        
            // RenderTexture.activeの内容をtextureに書き込み
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            RenderTexture.active = null;
        
            // 不要になったので削除
            RenderTexture.DestroyImmediate(renderTexture);
        }

        void MakeMaskTexture(Texture input_Tex,Texture input_Mask){
            string shaderFolderName = null;

            Texture2D texture = 
            new Texture2D(input_Tex.width, input_Tex.height, TextureFormat.RGBA32, false);

            TransferTextureToTexture2D(ref texture,input_Tex);

            //マスクテクスチャをTexture2Dへ変換する
            Texture2D mask = 
            new Texture2D(input_Mask.width, input_Mask.height, TextureFormat.RGBA32, false);

            TransferTextureToTexture2D(ref mask,input_Mask);

            Color[] pixels = texture.GetPixels();
            Color[] pixels_mask = mask.GetPixels();
            

            for (int i = 0; i < pixels.Length; i++)
            {
                if(pixels_mask[i].a > 0.0f){
                    pixels[i] = new Color(0,0,0,0);
                }
                else{
                    
                }
            }

            texture.SetPixels(pixels);

            shaderFolderName = ShaderName_MToon.Replace('/', '_');

            // pngとして保存
            System.IO.File.WriteAllBytes(OutputFolderName + "/" + targetModel.name + "/" + TextureFolderName + "/"
            +  shaderFolderName + "/" + input_Tex.name + ".png", texture.EncodeToPNG());
            
            AssetDatabase.Refresh();
        }

        void SettingProperty_FromUTS2ToMToon(ref Material material,Texture input_Tex,Shader changeShader){
            string shaderFolderName = null;

            Texture normalMap = material.GetTexture("_NormalMap");

            Texture matCap = material.GetTexture("_MatCap_Sampler");
            Texture emissionMap = material.GetTexture("_Emissive_Tex");
            
            Color mainColor = material.GetColor("_BaseColor");
            Color shadeColor = material.GetColor("_1st_ShadeColor");

            Color emissionColor = material.GetColor("_Emissive_Color");
            float outlineWidth = material.GetFloat("_Outline_Width");
            Color outlineColor = material.GetColor("_Outline_Color");

            material.shader = changeShader;

            shaderFolderName = ShaderName_MToon.Replace('/', '_');

            //テクスチャはマスク箇所を加味して作成したテクスチャを使用する
            Texture mainTex = Resources.Load(targetModel.name + "/" + TextureFolderName +  "/"
            + shaderFolderName + "/" + input_Tex.name ) as Texture;
            material.SetTexture("_MainTex",mainTex);

            Texture ShadeTex = Resources.Load(targetModel.name + "/" + TextureFolderName +  "/" 
            + shaderFolderName + "/" + input_Tex.name ) as Texture;
            material.SetTexture("_ShadeTexture",ShadeTex);

            material.SetTexture("_BumpMap",normalMap);
            material.SetColor("_Color",mainColor);
            material.SetColor("_ShadeColor",shadeColor);

            shaderFolderName = ShaderName_UTS2.Replace('/', '_');

            material.SetTexture("_SphereAdd",matCap);
            material.SetTexture("_EmissionMap",emissionMap);
            material.SetColor("_EmissionColor",emissionColor);
            material.SetFloat("_OutlineWidth",outlineWidth);
            material.SetColor("_OutlineColor",outlineColor);            
        }

        void SaveMaterial(Material material,string ShaderName){
            Material outputMaterial = new Material(Shader.Find(ShaderName));
            outputMaterial.CopyPropertiesFromMaterial(material);

            string shaderFolderName = ShaderName.Replace('/', '_');

            AssetDatabase.CreateAsset(outputMaterial, OutputFolderName + "/" + targetModel.name + "/" 
            + MaterialFolderName + "/" + shaderFolderName + "/" + material.name + ".mat");
        }
    }

    public static class Change_UTS2ToMToon_Menu
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/MaterialSetting/Change_UTS2ToMToon";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void Menu()
        {
           Change_UTS2ToMToon.CreateWizard();
        }
    }
}
