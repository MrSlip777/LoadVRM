/*
 * 対象のモデルにマテリアルを設定するスクリプト
 *
 * (C)2019 slip
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

namespace VRM
{

    public class ApplyMaterialSetting : ScriptableWizard
    {
        static public Object[] objects;
        static public MaterialSetting setting;
        static public GameObject parentObject;
        static public Material[] materials;
        public string[] MaterialName = null;
       
        public static void CreateWizard()
        {
            ApplyMaterialSetting.objects = Resources.LoadAll("MaterialsSetting");

            var wiz = ScriptableWizard.DisplayWizard<ApplyMaterialSetting>(
                "ApplyMaterialSetting", "Apply");
            var go = Selection.activeObject as GameObject;

        }

        void OnWizardCreate()
        {
            for(int i = 0;  i<objects.Length; i++){
                setting = (MaterialSetting)objects[i];
                GameObject targetObject = GameObject.Find(setting.targetName);
                materials = targetObject.GetComponent<SkinnedMeshRenderer>().sharedMaterials;

                for(int j = 0;  j<materials.Length; j++){
                    materials[j] = setting.mat[j];
                }

                targetObject.GetComponent<SkinnedMeshRenderer>().materials = materials;
            }
        }

        void OnWizardUpdate()
        {
            if(MaterialName == null){
                MaterialName =  new string[objects.Length];
                for(int i = 0;  i<objects.Length; i++){
                    MaterialSetting materialSetting = (MaterialSetting)objects[i];
                    MaterialName[i] = materialSetting.targetName;
                }
            }
        }
    }

    public static class ApplyMaterialSettingMenu
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/ApplyMaterialSetting";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void Menu()
        {
           ApplyMaterialSetting.CreateWizard();
        }
    }
}