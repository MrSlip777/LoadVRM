/*
 * VRMSpringBone、VRMSpringBoneCollider設定をScriptableObjectとして出力するスクリプト
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
    public class ExportVRMSpringBoneSetting : ScriptableWizard
    {
        //対象になるモデル
        public GameObject targetModel;

        private List<string> ColliderTergetName = new List<string>();

        private static GameObject m_Wizard;
        static private ReflectSettingUtility m_Utility;

        public static void CreateWizard()
        {

            var wiz = ScriptableWizard.DisplayWizard<ExportVRMSpringBoneSetting>(
                "ExportVRMSpringBoneSetting", "export");
            var go = Selection.activeObject as GameObject;
            m_Wizard = new GameObject();
            m_Utility = m_Wizard.AddComponent<ReflectSettingUtility>();
        }

        void OnWizardCreate()
        {
            //SpringBoneの設定値をScriptableObjectとして保存する
            ExportVRMSpringBone();
            //SpringBoneColliderをScriptableObjectとして保存する
            ExportVRMSpringBoneCollider();
        }

        void ExportVRMSpringBone(){

            Object[] objects = targetModel.GetComponentsInChildren<VRMSpringBone>();

            //gizmo以外はエクスポート　
            //transformは名前をエクスポートする(インポート時にFindで探す)
            for(int j = 0; j < objects.Length; j++){
                VRMSpringBone springbone = (VRMSpringBone)objects[j];

                VRMSpringBoneSetting exportData
                = ScriptableObject.CreateInstance<VRMSpringBoneSetting>();

                exportData.m_AttachObject = m_Utility.GetHierarchyPath(springbone.gameObject.transform);

                exportData.m_comment = springbone.m_comment;
                exportData.m_stiffnessForce = springbone.m_stiffnessForce;
                exportData.m_gravityPower = springbone.m_gravityPower;
                exportData.m_gravityDir = springbone.m_gravityDir;
                exportData.m_dragForce = springbone.m_dragForce;
                if(springbone.m_center != null){
                    exportData.m_center = m_Utility.GetHierarchyPath(springbone.m_center.transform);
                }
                exportData.RootBones = new string[springbone.RootBones.Count];
                for(int i = 0; i<springbone.RootBones.Count; i++){
                    exportData.RootBones[i] = m_Utility.GetHierarchyPath(springbone.RootBones[i].transform);
                }
                exportData.m_hitRadius = springbone.m_hitRadius;

                exportData.ColliderGroups = new string[springbone.ColliderGroups.Length];
                for(int i = 0; i<springbone.ColliderGroups.Length; i++){
                
                    exportData.ColliderGroups[i]
                    = m_Utility.GetHierarchyPath(springbone.ColliderGroups[i].transform);

                    if(ColliderTergetName.Count == 0){
                        ColliderTergetName.Add(exportData.ColliderGroups[i]);
                    }
                    else{
                        if(ColliderTergetName.IndexOf(exportData.ColliderGroups[i]) == -1){
                            ColliderTergetName.Add(exportData.ColliderGroups[i]);
                        };
                    }
                }

                AssetDatabase.CreateAsset(
                    exportData, "Assets/Models/Resources/SpringBoneData/SpringBone/VRMSpringBoneData_"+j+".asset");
            }
        }

        void ExportVRMSpringBoneCollider(){
            //gizmo以外はエクスポート
            for(int j = 0; j<ColliderTergetName.Count; j++){
                GameObject colliderObject = GameObject.Find(ColliderTergetName[j]);
                VRMSpringBoneColliderGroup collider = colliderObject.GetComponent<VRMSpringBoneColliderGroup>();
                VRMSpringBoneColliderSetting exportData
                = ScriptableObject.CreateInstance<VRMSpringBoneColliderSetting>();
                exportData.TargetName = m_Utility.GetHierarchyPath(colliderObject.transform);
                exportData.Colliders = new VRMSpringBoneColliderSetting.SphereCollider[collider.Colliders.Length];
                for(int i = 0; i<collider.Colliders.Length; i++){
                    exportData.Colliders[i] = new VRMSpringBoneColliderSetting.SphereCollider();
                    exportData.Colliders[i].Radius = collider.Colliders[i].Radius;
                    exportData.Colliders[i].Offset = collider.Colliders[i].Offset;
                }

                AssetDatabase.CreateAsset(
                        exportData, "Assets/Models/Resources/SpringBoneData/Collider/VRMSpringBoneColliderData_"+j+".asset");
            }
        }

        void OnWizardUpdate()
        {

        }

        void OnDestroy(){
            DestroyImmediate(m_Wizard);
        }
    }

    public static class ExportMenu
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/VRMSpringBone/ExportSetting";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void ExportSettingMenu()
        {
           ExportVRMSpringBoneSetting.CreateWizard();
        }
    }
}

#endif