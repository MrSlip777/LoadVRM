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

#if UNITY_EDITOR

using System.Text;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using VRM;
using UnityEditor;

namespace VRM
{
    public class ApplyVRMSpringBoneSetting : ScriptableWizard
    {
        //対象になるモデル
        public GameObject targetModel;

        static public Object[] boneObjects;
        private List<string> ColliderTergetName = new List<string>();
        static public Object[] colliderObjects;
        public string[] BoneSettingName = null;
        public string[] ColliderSettingName = null;

        private static GameObject m_Wizard;
        private static RemoveVRMSpringBone m_RemoveVRMSpringBone;
        
        static private ReflectSettingUtility m_Utility;
               

        public static void CreateWizard()
        {
            boneObjects = Resources.LoadAll("SpringBoneData/SpringBone");
            colliderObjects = Resources.LoadAll("SpringBoneData/Collider");

            var wiz = ScriptableWizard.DisplayWizard<ApplyVRMSpringBoneSetting>(
                "ApplyVRMSpringBoneSetting", "Apply");
            GameObject go = Selection.activeObject as GameObject;

            m_Wizard = new GameObject();
            m_RemoveVRMSpringBone = m_Wizard.AddComponent<RemoveVRMSpringBone>();
            m_Utility = m_Wizard.AddComponent<ReflectSettingUtility>();
        }

        void OnWizardCreate()
        {
             //ボーンとコライダーを削除
            m_RemoveVRMSpringBone.Remove(targetModel);

            //設定ファイルに基づいてSpringBoneColliderを設定する
            ApplyVRMSpringBoneCollider();
            //設定ファイルに基づいてSpringBoneを設定する
            ApplyVRMSpringBone();
        }

        void ApplyVRMSpringBone(){

            //gizmo以外は設定を反映
            //transformはFind後したTransformを反映)
            for(int j = 0; j < boneObjects.Length; j++){
                //設定データ
                VRMSpringBoneSetting settingData = (VRMSpringBoneSetting)boneObjects[j];
                
                //対象のボーン
                //m_Utility.ChangeRootPath 設定されたパスをターゲット対象に変更する
                GameObject targetObject
                 = GameObject.Find(m_Utility.ChangeRootPath(settingData.m_AttachObject,targetModel.name));

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
                    GameObject findObject
                     = GameObject.Find(m_Utility.ChangeRootPath(settingData.RootBones[i],targetModel.name));

                    springbone.RootBones.Add(findObject.GetComponent<Transform>());
                }
                springbone.m_hitRadius = settingData.m_hitRadius;
                springbone.ColliderGroups = new VRMSpringBoneColliderGroup[settingData.ColliderGroups.Length];
                for(int i = 0; i<settingData.ColliderGroups.Length; i++){
                    GameObject findObject
                     = GameObject.Find(m_Utility.ChangeRootPath(settingData.ColliderGroups[i],targetModel.name));
                     
                    springbone.ColliderGroups[i] = findObject.GetComponent<VRMSpringBoneColliderGroup>();
                }
            }
        }

        void ApplyVRMSpringBoneCollider(){
            //gizmo以外は設定を反映する
            for(int j = 0; j<colliderObjects.Length; j++){
                VRMSpringBoneColliderSetting settingData = (VRMSpringBoneColliderSetting)colliderObjects[j];

                //対象のボーン
                //m_Utility.ChangeRootPath 設定されたパスをターゲット対象に変更する
                GameObject targetObject
                 = GameObject.Find(m_Utility.ChangeRootPath(settingData.TargetName,targetModel.name));

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
            if(BoneSettingName == null){
                BoneSettingName =  new string[boneObjects.Length];
                for(int i = 0;  i<boneObjects.Length; i++){
                    VRMSpringBoneSetting setting = (VRMSpringBoneSetting)boneObjects[i];
                    
                    if(setting.m_comment == ""){
                        
                        foreach(string name in setting.RootBones){
                            BoneSettingName[i] = "右のボーンを含む→" + name;
                            break;
                        }
                    }
                    else{
                        BoneSettingName[i] = setting.m_comment;
                    }
                }
            }
            if(ColliderSettingName == null){
                ColliderSettingName =  new string[colliderObjects.Length];
                for(int i = 0;  i<colliderObjects.Length; i++){
                    VRMSpringBoneColliderSetting setting
                     = (VRMSpringBoneColliderSetting)colliderObjects[i];
                    ColliderSettingName[i] = setting.TargetName;
                }
            }
        }

        void OnDestroy(){
            DestroyImmediate(m_Wizard);
        }         
    }

    public static class ApplyMenu
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/VRMSpringBone/ApplySetting";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void ApplySettingMenu()
        {
           ApplyVRMSpringBoneSetting.CreateWizard();
        }
    }
}

#endif