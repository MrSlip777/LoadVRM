/*
 * DynamicBone、DynamicBoneCollider設定をScriptableObjectとして出力するスクリプト
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
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace VRM
{
    public class ExportDynamicBoneSetting : ScriptableWizard
    {
        static public Object[] objects;
        public List<string> ColliderTergetName = new List<string>();
        static public Object[] colliderObjects;

        public static void CreateWizard()
        {

            var wiz = ScriptableWizard.DisplayWizard<ExportDynamicBoneSetting>(
                "ExportDynamicBoneSetting", "export");
            var go = Selection.activeObject as GameObject;

        }

        void OnWizardCreate()
        {
            //SpringBoneの設定値をScriptableObjectとして保存する
            ExportDynamicBone();
            //SpringBoneColliderをScriptableObjectとして保存する
            ExportDynamicBoneCollider();
        }

        void ExportDynamicBone(){
            //モデル上でsecondaryの部分を探す
            GameObject targetObject = GameObject.Find("secondary");
            objects = targetObject.GetComponents<DynamicBone>();

            //gizmo以外はエクスポート　
            //transformは名前をエクスポートする(インポート時にFindで探す)
            for(int j = 0; j < objects.Length; j++){
                DynamicBone dynamicbone = (DynamicBone)objects[j];

                DynamicBoneSetting exportData
                = ScriptableObject.CreateInstance<DynamicBoneSetting>();

                if(dynamicbone.m_Root != null){
                    exportData.m_Root = dynamicbone.m_Root.name;
                }

                exportData.m_UpdateRate = dynamicbone.m_UpdateRate;
                exportData.m_UpdateMode = dynamicbone.m_UpdateMode;
                exportData.m_Damping = dynamicbone.m_Damping;
                exportData.m_DampingDistrib = dynamicbone.m_DampingDistrib;
                exportData.m_Elasticity = dynamicbone.m_Elasticity;
                exportData.m_ElasticityDistrib  = dynamicbone.m_ElasticityDistrib;
                exportData.m_Stiffness = dynamicbone.m_Stiffness;
                exportData.m_StiffnessDistrib = dynamicbone.m_StiffnessDistrib;
                exportData.m_Inert = dynamicbone.m_Inert;
                exportData.m_InertDistrib = dynamicbone.m_InertDistrib;
                exportData.m_Radius = dynamicbone.m_Radius;
                exportData.m_RadiusDistrib = dynamicbone.m_RadiusDistrib;
                exportData.m_EndLength = dynamicbone.m_EndLength;
                exportData.m_EndOffset = dynamicbone.m_EndOffset;
                exportData.m_Gravity = dynamicbone.m_Gravity;
                exportData.m_Force = dynamicbone.m_Force;

                exportData.m_Colliders = new List<string>();
                foreach(DynamicBoneCollider collider in dynamicbone.m_Colliders){
                    exportData.m_Colliders.Add(collider.name);

                    //コライダーのついているオブジェクトの名前を登録する（重複なし、なければ追加）
                    if(ColliderTergetName.Count == 0){
                        ColliderTergetName.Add(collider.name);
                    }
                    else{
                        if(ColliderTergetName.IndexOf(collider.name) == -1){
                            ColliderTergetName.Add(collider.name);
                        };
                    }
                }

                exportData.m_Exclusions = new List<string>();
                if(dynamicbone.m_Exclusions != null){
                    
                    foreach(Transform exclusion in dynamicbone.m_Exclusions){
                        exportData.m_Exclusions.Add(exclusion.name);
                    }
                }

                exportData.m_FreezeAxis = dynamicbone.m_FreezeAxis;
                exportData.m_DistantDisable = dynamicbone.m_DistantDisable;

                if(dynamicbone.m_ReferenceObject != null){
                    exportData.m_ReferenceObject = dynamicbone.m_ReferenceObject.name;
                }

                exportData.m_DistanceToObject = dynamicbone.m_DistanceToObject;

                AssetDatabase.CreateAsset(
                    exportData, "Assets/Models/Resources/DynamicBoneData/DynamicBone/DynamicBoneData_"+j+".asset");
            }
        }

        void ExportDynamicBoneCollider(){
            //gizmo以外はエクスポート
            for(int j = 0; j<ColliderTergetName.Count; j++){
                GameObject colliderObject = GameObject.Find(ColliderTergetName[j]);
                DynamicBoneCollider[] colliders = colliderObject.GetComponents<DynamicBoneCollider>();
                DynamicBoneColliderSetting exportData
                = ScriptableObject.CreateInstance<DynamicBoneColliderSetting>();
                exportData.TargetName = colliderObject.name;

                exportData.Colliders = new DynamicBoneColliderSetting.SphereCollider[colliders.Length];
                for(int i= 0; i<colliders.Length; i++){
                    exportData.Colliders[i] = new DynamicBoneColliderSetting.SphereCollider();
                    exportData.Colliders[i].m_Direction = colliders[i].m_Direction;
                    exportData.Colliders[i].m_Center = colliders[i].m_Center;
                    exportData.Colliders[i].m_Bound = colliders[i].m_Bound;
                    exportData.Colliders[i].m_Radius = colliders[i].m_Radius;
                    exportData.Colliders[i].m_Height = colliders[i].m_Height;
                }

                AssetDatabase.CreateAsset(
                        exportData, "Assets/Models/Resources/DynamicBoneData/Collider/DynamicBoneColliderData_"+j+".asset");
            }
        }

        void OnWizardUpdate()
        {

        }
    }

    public static class ExportMenuforDynamicBone
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/DynamicBone/ExportSetting";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void ExportSettingMenu()
        {
           ExportDynamicBoneSetting.CreateWizard();
        }
    }
}