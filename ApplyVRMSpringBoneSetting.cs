/*
 * VRMSpringBone、VRMSpringBoneCollider設定を反映するスクリプト
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
using VRM;

namespace VRM
{
    public class ApplyVRMSpringBoneSetting : ScriptableWizard
    {
        static public Object[] boneObjects;
        public List<string> ColliderTergetName = new List<string>();
        static public Object[] colliderObjects;

        public static void CreateWizard()
        {

            var wiz = ScriptableWizard.DisplayWizard<ApplyVRMSpringBoneSetting>(
                "ApplyVRMSpringBoneSetting", "Apply");
            var go = Selection.activeObject as GameObject;

        }

        void OnWizardCreate()
        {
            boneObjects = Resources.LoadAll("SpringBoneData/SpringBone");
            colliderObjects = Resources.LoadAll("SpringBoneData/Collider");
            //設定ファイルに基づいてSpringBoneColliderを設定する
            ApplyVRMSpringBoneCollider();
            //設定ファイルに基づいてSpringBoneを設定する
            ApplyVRMSpringBone();

        }

        void ApplyVRMSpringBone(){

            //モデル上でsecondaryの部分を探す
            GameObject targetObject = GameObject.Find("secondary");

            Debug.Log("number:" + boneObjects.Length);
            //gizmo以外は設定を反映
            //transformはFind後したTransformを反映)
            for(int j = 0; j < boneObjects.Length; j++){
                //設定データ
                VRMSpringBoneSetting settingData = (VRMSpringBoneSetting)boneObjects[j];
                //対象のボーン
                VRMSpringBone springbone = targetObject.AddComponent<VRMSpringBone>();
                
                springbone.m_comment = settingData.m_comment;
                springbone.m_stiffnessForce = settingData.m_stiffnessForce;
                springbone.m_gravityPower = settingData.m_gravityPower;
                springbone.m_gravityDir = settingData.m_gravityDir;
                springbone.m_dragForce = settingData.m_dragForce;
                if(settingData.m_center != null){
                    GameObject findObject = GameObject.Find(settingData.m_center);
                    if(findObject != null){
                        springbone.m_center = findObject.GetComponent<Transform>();
                    }
                }
                springbone.RootBones = new List<Transform>();
                for(int i = 0; i<settingData.RootBones.Length; i++){
                    GameObject findObject = GameObject.Find(settingData.RootBones[i]);
                    springbone.RootBones.Add(findObject.GetComponent<Transform>());
                }
                springbone.m_hitRadius = settingData.m_hitRadius;
                springbone.ColliderGroups = new VRMSpringBoneColliderGroup[settingData.ColliderGroups.Length];
                for(int i = 0; i<settingData.ColliderGroups.Length; i++){
                    GameObject findObject = GameObject.Find(settingData.ColliderGroups[i]);
                    springbone.ColliderGroups[i] = findObject.GetComponent<VRMSpringBoneColliderGroup>();
                }
            }
        }

        void ApplyVRMSpringBoneCollider(){
            //gizmo以外は設定を反映する
            for(int j = 0; j<colliderObjects.Length; j++){
                VRMSpringBoneColliderSetting settingData = (VRMSpringBoneColliderSetting)colliderObjects[j];
                GameObject targetObject = GameObject.Find(settingData.TargetName);
                VRMSpringBoneColliderGroup collider = targetObject.AddComponent<VRMSpringBoneColliderGroup>();
                
                collider.Colliders = new VRMSpringBoneColliderGroup.SphereCollider[settingData.Colliders.Length];
                for(int i = 0; i<settingData.Colliders.Length; i++){
                    
                    collider.Colliders[i] = new VRMSpringBoneColliderGroup.SphereCollider();
                    collider.Colliders[i].Radius = settingData.Colliders[i].Radius;
                    collider.Colliders[i].Offset = settingData.Colliders[i].Offset;
                }

            }
        }

        void OnWizardUpdate()
        {

        }
    }

    public static class ApplyMenu
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/ApplyVRMSpringBoneSetting";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void ApplySettingMenu()
        {
           ApplyVRMSpringBoneSetting.CreateWizard();
        }
    }
}