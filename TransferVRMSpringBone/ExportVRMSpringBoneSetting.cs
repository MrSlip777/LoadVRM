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
        static public Object[] objects;
        public List<string> ColliderTergetName = new List<string>();
        static public Object[] colliderObjects;

        public static void CreateWizard()
        {

            var wiz = ScriptableWizard.DisplayWizard<ExportVRMSpringBoneSetting>(
                "ExportVRMSpringBoneSetting", "export");
            var go = Selection.activeObject as GameObject;

        }

        void OnWizardCreate()
        {
            //SpringBoneの設定値をScriptableObjectとして保存する
            ExportVRMSpringBone();
            //SpringBoneColliderをScriptableObjectとして保存する
            ExportVRMSpringBoneCollider();
        }

        void ExportVRMSpringBone(){
            //モデル上でsecondaryの部分を探す
            GameObject targetObject = GameObject.Find("secondary");
            objects = targetObject.GetComponents<VRMSpringBone>();

            //gizmo以外はエクスポート　
            //transformは名前をエクスポートする(インポート時にFindで探す)
            for(int j = 0; j < objects.Length; j++){
                VRMSpringBone springbone = (VRMSpringBone)objects[j];

                VRMSpringBoneSetting exportData
                = ScriptableObject.CreateInstance<VRMSpringBoneSetting>();

                exportData.m_comment = springbone.m_comment;
                exportData.m_stiffnessForce = springbone.m_stiffnessForce;
                exportData.m_gravityPower = springbone.m_gravityPower;
                exportData.m_gravityDir = springbone.m_gravityDir;
                exportData.m_dragForce = springbone.m_dragForce;
                if(springbone.m_center != null){
                    exportData.m_center = springbone.m_center.name;
                }
                exportData.RootBones = new string[springbone.RootBones.Count];
                for(int i = 0; i<springbone.RootBones.Count; i++){
                    exportData.RootBones[i] = springbone.RootBones[i].name;
                }
                exportData.m_hitRadius = springbone.m_hitRadius;
                exportData.ColliderGroups = new string[springbone.ColliderGroups.Length];
                for(int i = 0; i<springbone.ColliderGroups.Length; i++){
                    exportData.ColliderGroups[i] = springbone.ColliderGroups[i].name;
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
                exportData.TargetName = colliderObject.name;
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