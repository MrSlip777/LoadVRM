/*
 * 対象のモデルにマテリアルを取得するスクリプト
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

    public class ExportMaterialSetting : ScriptableWizard
    {
        //対象になるモデル
        public GameObject targetModel;

        static public Object[] objects;
        static public MaterialSetting setting;
        static public GameObject parentObject;
        //static public Material[] materials;
        public string[] MaterialName = null;
        private static GameObject m_Wizard;
        static private ReflectSettingUtility m_Utility;
       
        public static void CreateWizard()
        {
            var wiz = ScriptableWizard.DisplayWizard<ExportMaterialSetting>(
                "ExportMaterialSetting", "Export");
            var go = Selection.activeObject as GameObject;
            m_Wizard = new GameObject();
            m_Utility = m_Wizard.AddComponent<ReflectSettingUtility>();
        }

        void OnWizardCreate()
        {
            SkinnedMeshRenderer[] skinnedMeshRenderers
             = targetModel.GetComponentsInChildren<SkinnedMeshRenderer>();

            for(int j = 0; j<skinnedMeshRenderers.Length; j++){
                MaterialSetting exportData
                = ScriptableObject.CreateInstance<MaterialSetting>();

                exportData.targetName
                 = m_Utility.GetHierarchyPath(skinnedMeshRenderers[j].gameObject.transform);

                Material[] materials = skinnedMeshRenderers[j].sharedMaterials;

                exportData.mat = new Material[materials.Length];

                for(int i = 0; i<materials.Length; i++){
                    exportData.mat[i] = materials[i];
                }

                AssetDatabase.CreateAsset(
                    exportData, "Assets/Models/Resources/MaterialData/MaterialSetting_"+j+".asset");

            }


        }

        void OnDestroy(){
            DestroyImmediate(m_Wizard);
        }           
    }

    public static class ExportMaterialSettingMenu
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/ExportMaterialSetting";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void Menu()
        {
           ExportMaterialSetting.CreateWizard();
        }
    }
}