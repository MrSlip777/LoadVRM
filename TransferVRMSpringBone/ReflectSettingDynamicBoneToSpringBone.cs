/*
 * DynamicBone設定をVRMSpringBoneへ反映、置換するスクリプト
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
 *  Slip
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
    public class ReflectSettingDynamicBoneToSpringBone  : ScriptableWizard
    {
        //対象になるモデル
        public GameObject targetModel;

        static public Object[] boneObjects;
        private List<string> ColliderTergetName = new List<string>();
        static public Object[] colliderObjects;
        public string[] BoneSettingName = null;
        public string[] ColliderSettingName = null;
        
        private static GameObject m_Wizard;
        private static RemoveDynamicBone m_RemoveDynamicBone;
        private static RemoveVRMSpringBone m_RemoveVRMSpringBone;

        public static void CreateWizard()
        {
            boneObjects = Resources.LoadAll("DynamicBoneData/DynamicBone");
            colliderObjects = Resources.LoadAll("DynamicBoneData/Collider");

            var wiz = ScriptableWizard.DisplayWizard<ReflectSettingDynamicBoneToSpringBone>(
                "ReflectSettingDynamicBoneToSpringBone", "Apply");
            var go = Selection.activeObject as GameObject;

            m_Wizard = new GameObject();
            m_RemoveDynamicBone = m_Wizard.AddComponent<RemoveDynamicBone>();
            m_RemoveVRMSpringBone = m_Wizard.AddComponent<RemoveVRMSpringBone>();
        }

        void OnWizardCreate()
        {
            //ボーンとコライダーを削除
            m_RemoveDynamicBone.Remove(targetModel);
            m_RemoveVRMSpringBone.Remove(targetModel);

            //設定ファイルに基づいてSpringBoneColliderを設定する
            ApplyVRMSpringBoneCollider();
            //設定ファイルに基づいてSpringBoneを設定する
            ApplyVRMSpringBone();
        }

        void ApplyVRMSpringBone(){

            for(int j = 0; j < boneObjects.Length; j++){
                GameObject findObject = null;
                //設定データ
                DynamicBoneSetting settingData = (DynamicBoneSetting)boneObjects[j];
                //対象のボーン
                GameObject targetObject = GameObject.Find(settingData.m_AttachObject);
                VRMSpringBone springbone = targetObject.AddComponent<VRMSpringBone>();
                
                springbone.m_stiffnessForce = settingData.m_Stiffness * 4.0f;

                //重力の設定　大きさが0であればY方向-1に設定
                //大きさは2以上は2にする
                springbone.m_gravityPower = settingData.m_Gravity.magnitude;

                if(springbone.m_gravityPower>2.0f){
                    springbone.m_gravityPower = 2.0f;
                }

                if(springbone.m_gravityPower != 0){
                    springbone.m_gravityDir = settingData.m_Gravity/settingData.m_Gravity.magnitude;
                }
                else{
                    springbone.m_gravityDir = new Vector3(0,-1.0f,0);
                }

                springbone.RootBones = new List<Transform>();
                findObject = GameObject.Find(settingData.m_Root);
                springbone.RootBones.Add(findObject.GetComponent<Transform>());

                springbone.m_hitRadius = settingData.m_Radius;

                springbone.ColliderGroups = new VRMSpringBoneColliderGroup[settingData.m_Colliders.Count];
                for(int i = 0; i<settingData.m_Colliders.Count; i++){
                    findObject = GameObject.Find(settingData.m_Colliders[i]);
                    springbone.ColliderGroups[i] = findObject.GetComponent<VRMSpringBoneColliderGroup>();
                }
            }
        }

        void ApplyVRMSpringBoneCollider(){
            //gizmo以外は設定を反映する
            for(int j = 0; j<colliderObjects.Length; j++){
                DynamicBoneColliderSetting settingData = (DynamicBoneColliderSetting)colliderObjects[j];
                GameObject targetObject = GameObject.Find(settingData.TargetName);

                VRMSpringBoneColliderGroup collider = targetObject.AddComponent<VRMSpringBoneColliderGroup>();
                
                collider.Colliders = new VRMSpringBoneColliderGroup.SphereCollider[settingData.Colliders.Length];
                for(int i = 0; i<settingData.Colliders.Length; i++){
                    
                    collider.Colliders[i] = new VRMSpringBoneColliderGroup.SphereCollider();
                    collider.Colliders[i].Radius = settingData.Colliders[i].m_Radius;
                    collider.Colliders[i].Offset = settingData.Colliders[i].m_Center;
                }
            }
        }

        void OnWizardUpdate()
        {
            if(BoneSettingName == null){
                BoneSettingName =  new string[boneObjects.Length];
                for(int i = 0;  i<boneObjects.Length; i++){
                    DynamicBoneSetting setting = (DynamicBoneSetting)boneObjects[i];

                    if(setting.m_Root != null){
                        BoneSettingName[i] = setting.m_Root.ToString();
                    }
                }
            }
            if(ColliderSettingName == null){
                ColliderSettingName =  new string[colliderObjects.Length];
                for(int i = 0;  i<colliderObjects.Length; i++){
                    DynamicBoneColliderSetting setting
                     = (DynamicBoneColliderSetting)colliderObjects[i];
                    ColliderSettingName[i] = setting.TargetName;
                }
            }
        }

        void OnDestroy(){
            DestroyImmediate(m_Wizard);
        }                     
    }

    public static class RelfectVRMSpringBoneMenu
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/ReplaceBone/DynamicBoneToSpringBone";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void RelfectMenu()
        {
           ReflectSettingDynamicBoneToSpringBone.CreateWizard();
        }
    }    
}