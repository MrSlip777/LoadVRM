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

#if UNITY_EDITOR

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
        //対象になるモデル
        public GameObject targetModel;

        private List<string> ColliderTergetName = new List<string>();
        static private Object[] colliderObjects;

        private static GameObject m_Wizard;
        static private ReflectSettingUtility m_Utility;

        public static void CreateWizard()
        {

            var wiz = ScriptableWizard.DisplayWizard<ExportDynamicBoneSetting>(
                "ExportDynamicBoneSetting", "export");
            var go = Selection.activeObject as GameObject;
            m_Wizard = new GameObject();
            m_Utility = m_Wizard.AddComponent<ReflectSettingUtility>();
        }

        void OnWizardCreate()
        {
            //SpringBoneの設定値をScriptableObjectとして保存する
            ExportDynamicBone();
            //SpringBoneColliderをScriptableObjectとして保存する
            ExportDynamicBoneCollider();
        }

        void ExportDynamicBone(){
            
            DynamicBone[]　bones = targetModel.GetComponentsInChildren<DynamicBone>();

            //gizmo以外はエクスポート　
            //transformは名前をエクスポートする(インポート時にFindで探す)
            for(int j = 0; j < bones.Length; j++){
                DynamicBone dynamicbone = bones[j];

                DynamicBoneSetting exportData
                = ScriptableObject.CreateInstance<DynamicBoneSetting>();

                //フルパスを取得する
                exportData.m_AttachObject = m_Utility.GetHierarchyPath(dynamicbone.gameObject.transform);

                if(dynamicbone.m_Root != null){
                    exportData.m_Root = m_Utility.GetHierarchyPath(dynamicbone.m_Root);
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
                    //紐付けているコライダーがある場合
                    if(collider != null){
                    
                        string colliderName = m_Utility.GetHierarchyPath(collider.gameObject.transform);
                        exportData.m_Colliders.Add(colliderName);

                        //コライダーのついているオブジェクトの名前を登録する（重複なし、なければ追加）
                        if(ColliderTergetName.Count == 0){
                            ColliderTergetName.Add(colliderName);
                        }
                        else{
                            if(ColliderTergetName.IndexOf(colliderName) == -1){
                                ColliderTergetName.Add(colliderName);
                            };
                        }
                    }
                }

                exportData.m_Exclusions = new List<string>();
                if(dynamicbone.m_Exclusions != null){
                    
                    foreach(Transform exclusion in dynamicbone.m_Exclusions){
                        exportData.m_Exclusions.Add(m_Utility.GetHierarchyPath(exclusion));
                    }
                }

                exportData.m_FreezeAxis = dynamicbone.m_FreezeAxis;
                exportData.m_DistantDisable = dynamicbone.m_DistantDisable;

                if(dynamicbone.m_ReferenceObject != null){
                    exportData.m_ReferenceObject
                     = m_Utility.GetHierarchyPath(dynamicbone.m_ReferenceObject.transform);
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
                exportData.TargetName = m_Utility.GetHierarchyPath(colliderObject.transform);

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

        void OnDestroy(){
            DestroyImmediate(m_Wizard);
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

#endif