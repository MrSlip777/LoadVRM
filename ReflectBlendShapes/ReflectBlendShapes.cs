/*
 * 対象モデルのBlendShapeに他のBlendShape設定を反映させるスクリプト
 *
 * (C)2021 slip
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


using UnityEditor;
using UnityEngine;
using System;
using System.IO;

namespace VRM
{

    public class ReplaceFile : ScriptableWizard
    {

        //対象になるブレンドシェイプ
        public BlendShapeAvatar sourceBlendShape;
        public BlendShapeAvatar destinationBlendShape;

        readonly string RefName = "BlendShape.asset";

        public static void CreateWizard()
        {
            var wiz = ScriptableWizard.DisplayWizard<ReplaceFile>(
                "ApplyBlendShape", "Apply");
        }

        void OnWizardCreate()
        {
            string projectPath = null;
            string srcFullPath = null;
            string srcDir = null;
            string[] srcFiles = null;

            string dstFullPath = null;
            string dstDir = null;
            string[] dstFiles = null;

            projectPath = Application.dataPath.Replace("Assets","");
            
            srcFullPath = projectPath + AssetDatabase.GetAssetPath(sourceBlendShape);
            srcDir = System.IO.Path.GetDirectoryName(srcFullPath);
            srcFiles = System.IO.Directory.GetFiles(srcDir,"*");

            dstFullPath = projectPath + AssetDatabase.GetAssetPath(destinationBlendShape);
            dstDir = System.IO.Path.GetDirectoryName(dstFullPath);
            dstFiles = System.IO.Directory.GetFiles(dstDir,"*");

            //Unity内で使用できるようにする
            srcDir = srcDir.Replace("\\","/");
            dstDir = dstDir.Replace("\\","/");

            


            foreach (var item in srcFiles)
            {
                string srcPath = null;
                string dstPath = null;
                string srcFileName = null;
                string dstFileName = null;

                if(sourceBlendShape.Clips[0].name
                == destinationBlendShape.Clips[0].name){
                    srcFileName = System.IO.Path.GetFileName(item);
                    dstFileName =  srcFileName;                   
                }
                else{
                    if(sourceBlendShape.Clips[0].name.Contains("BlendShape.")){
                        srcFileName = System.IO.Path.GetFileName(item);
                        dstFileName = srcFileName.Replace("BlendShape.","");
                    }
                    else{
                        srcFileName = System.IO.Path.GetFileName(item);
                        dstFileName = "BlendShape." + srcFileName;
                    }
                }

                //BlendShape.assetでなければ、コピーする（各ブレンドシェイプをコピーする）
                if(srcFileName != RefName && srcFileName.Contains(".meta") == false){

                    srcPath = srcDir + "/" + srcFileName;
                    dstPath = dstDir + "/" + dstFileName;
                
                    File.Copy(srcPath, dstPath,true);

                }
            }

            AssetDatabase.Refresh();
        }
    }

    public static class ReplaceFileMenu
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/ReplaceBlendShapes";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void Menu()
        {
            ReplaceFile.CreateWizard();
        }
    }
}

#endif