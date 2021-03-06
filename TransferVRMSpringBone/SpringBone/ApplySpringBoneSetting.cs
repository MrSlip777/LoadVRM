﻿/*
 * SpringBone、SpringBoneCollider設定を反映するスクリプト
 *
 * (C)2020 slip
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
    public class ApplySpringBoneSetting : ScriptableWizard
    {
        //対象になるモデル
        public GameObject targetModel;

        static public Object[] boneObjects;
        private List<string> ColliderTergetName = new List<string>();
        static public Object[] colliderObjects;
        public string[] BoneSettingName = null;
        public string[] ColliderSettingName = null;

        private static GameObject m_Wizard;
        private static RemoveSpringBone m_RemoveSpringBone;
        
        static private ReflectSettingUtility m_Utility;
               

        public static void CreateWizard()
        {
            boneObjects = Resources.LoadAll("SpringBoneData/SpringBone");
            colliderObjects = Resources.LoadAll("SpringBoneData/Collider");

            var wiz = ScriptableWizard.DisplayWizard<ApplySpringBoneSetting>(
                "ApplySpringBoneSetting", "Apply");
            GameObject go = Selection.activeObject as GameObject;

            m_Wizard = new GameObject();
            m_RemoveSpringBone = m_Wizard.AddComponent<RemoveSpringBone>();
            m_Utility = m_Wizard.AddComponent<ReflectSettingUtility>();
        }

        void OnWizardCreate()
        {
             //ボーンとコライダーを削除
            m_RemoveSpringBone.Remove(targetModel);

            //設定ファイルに基づいてSpringBoneColliderを設定する
            ApplySpringBoneCollider();
            //設定ファイルに基づいてSpringBoneを設定する
            ApplySpringBone();
        }

        void ApplySpringBone(){

            //gizmo以外は設定を反映
            //transformはFind後したTransformを反映)
            for(int j = 0; j < boneObjects.Length; j++){
                //設定データ
                SpringBoneSetting settingData = (SpringBoneSetting)boneObjects[j];
                
                //対象のボーン
                //m_Utility.ChangeRootPath 設定されたパスをターゲット対象に変更する
                GameObject targetObject
                 = GameObject.Find(m_Utility.ChangeRootPath(settingData.m_AttachObject,targetModel.name));

                SpringBone springbone = targetObject.AddComponent<SpringBone>();
                
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
                springbone.ColliderGroups = new SpringBoneColliderGroup[settingData.ColliderGroups.Length];
                for(int i = 0; i<settingData.ColliderGroups.Length; i++){
                    GameObject findObject
                     = GameObject.Find(m_Utility.ChangeRootPath(settingData.ColliderGroups[i],targetModel.name));
                     
                    springbone.ColliderGroups[i] = findObject.GetComponent<SpringBoneColliderGroup>();
                }
            }
        }

        void ApplySpringBoneCollider(){
            //gizmo以外は設定を反映する
            for(int j = 0; j<colliderObjects.Length; j++){
                SpringBoneColliderSetting settingData = (SpringBoneColliderSetting)colliderObjects[j];

                //対象のボーン
                //m_Utility.ChangeRootPath 設定されたパスをターゲット対象に変更する
                GameObject targetObject
                 = GameObject.Find(m_Utility.ChangeRootPath(settingData.TargetName,targetModel.name));

                SpringBoneColliderGroup collider = targetObject.AddComponent<SpringBoneColliderGroup>();
                
                collider.Colliders = new SpringBoneColliderGroup.SphereCollider[settingData.Colliders.Length];
                for(int i = 0; i<settingData.Colliders.Length; i++){
                    
                    collider.Colliders[i] = new SpringBoneColliderGroup.SphereCollider();
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
                    SpringBoneSetting setting = (SpringBoneSetting)boneObjects[i];
                    
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
                    SpringBoneColliderSetting setting
                     = (SpringBoneColliderSetting)colliderObjects[i];
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
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/SpringBone/ApplySetting";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void ApplySettingMenu()
        {
           ApplySpringBoneSetting.CreateWizard();
        }
    }
}

#endif