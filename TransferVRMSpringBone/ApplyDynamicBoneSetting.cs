/*
 * DynamicBone、DynamicBoneCollider設定を反映するスクリプト
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
    public class ApplyDynamicBoneSetting : ScriptableWizard
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

        public static void CreateWizard()
        {
            boneObjects = Resources.LoadAll("dynamicboneData/dynamicbone");
            colliderObjects = Resources.LoadAll("dynamicboneData/Collider");

            var wiz = ScriptableWizard.DisplayWizard<ApplyDynamicBoneSetting>(
                "ApplyDynamicBoneSetting", "Apply");
            var go = Selection.activeObject as GameObject;

            m_Wizard = new GameObject();
            m_RemoveDynamicBone = m_Wizard.AddComponent<RemoveDynamicBone>();
        }

        void OnWizardCreate()
        {
            //ボーンとコライダーを削除
            m_RemoveDynamicBone.Remove(targetModel);

            //設定ファイルに基づいてdynamicboneColliderを設定する
            ApplyDynamicBoneCollider();
            //設定ファイルに基づいてdynamicboneを設定する
            ApplyDynamicBone();
        }

        void ApplyDynamicBone(){

            //gizmo以外は設定を反映
            //transformはFind後したTransformを反映)
            for(int j = 0; j < boneObjects.Length; j++){
                //設定データ
                DynamicBoneSetting settingData = (DynamicBoneSetting)boneObjects[j];

                //内部変数の定義
                GameObject findObject = null;

                //対象のボーン
                GameObject targetObject = GameObject.Find(settingData.m_AttachObject);
                DynamicBone dynamicbone = targetObject.AddComponent<DynamicBone>();

                findObject = GameObject.Find(settingData.m_Root);
                dynamicbone.m_Root = findObject.GetComponent<Transform>();

                dynamicbone.m_UpdateRate = settingData.m_UpdateRate;
                dynamicbone.m_UpdateMode = settingData.m_UpdateMode;
                dynamicbone.m_Damping = settingData.m_Damping;
                dynamicbone.m_DampingDistrib = settingData.m_DampingDistrib;
                dynamicbone.m_Elasticity = settingData.m_Elasticity;
                dynamicbone.m_ElasticityDistrib  = settingData.m_ElasticityDistrib;
                dynamicbone.m_Stiffness = settingData.m_Stiffness;
                dynamicbone.m_StiffnessDistrib = settingData.m_StiffnessDistrib;
                dynamicbone.m_Inert = settingData.m_Inert;
                dynamicbone.m_InertDistrib = settingData.m_InertDistrib;
                dynamicbone.m_Radius = settingData.m_Radius;
                dynamicbone.m_RadiusDistrib = settingData.m_RadiusDistrib;
                dynamicbone.m_EndLength = settingData.m_EndLength;
                dynamicbone.m_EndOffset = settingData.m_EndOffset;
                dynamicbone.m_Gravity = settingData.m_Gravity;
                dynamicbone.m_Force = settingData.m_Force;

                dynamicbone.m_Colliders = new List<DynamicBoneColliderBase>(); 

                if(settingData.m_Colliders != null){
                    foreach(string collider in settingData.m_Colliders){
                        findObject = GameObject.Find(collider);
                        if(findObject != null){
                            dynamicbone.m_Colliders.Add(findObject.GetComponent<DynamicBoneCollider>());
                        }
                    }
                }

                if(settingData.m_Exclusions != null){
                    dynamicbone.m_Exclusions = new List<Transform>();

                    foreach(string exclusion in settingData.m_Exclusions){
                        findObject = GameObject.Find(exclusion);
                        if(findObject != null){
                            dynamicbone.m_Exclusions.Add(findObject.GetComponent<Transform>());
                        }
                    }
                }

                dynamicbone.m_FreezeAxis = settingData.m_FreezeAxis;
                dynamicbone.m_DistantDisable = settingData.m_DistantDisable;

                if(settingData.m_ReferenceObject != null){
                    findObject = GameObject.Find(settingData.m_ReferenceObject);
                    if(findObject != null){
                        dynamicbone.m_ReferenceObject = findObject.GetComponent<Transform>();
                    }
                }

                dynamicbone.m_DistanceToObject = settingData.m_DistanceToObject;
            
            }
        }

        void ApplyDynamicBoneCollider(){
            //gizmo以外は設定を反映する
            for(int j = 0; j<colliderObjects.Length; j++){
                DynamicBoneColliderSetting settingData = (DynamicBoneColliderSetting)colliderObjects[j];

                GameObject targetObject = GameObject.Find(settingData.TargetName);

                for(int i = 0; i<settingData.Colliders.Length; i++){
                    DynamicBoneCollider collider = targetObject.AddComponent<DynamicBoneCollider>();
                    collider.m_Center = settingData.Colliders[i].m_Center;
                    collider.m_Radius = settingData.Colliders[i].m_Radius;
                    collider.m_Direction = settingData.Colliders[i].m_Direction;
                    collider.m_Bound = settingData.Colliders[i].m_Bound;
                    collider.m_Height = settingData.Colliders[i].m_Height;
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

    public static class ApplyMenuforDynamicBone
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/DynamicBone/ApplySetting";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void ApplySettingMenu()
        {
           ApplyDynamicBoneSetting.CreateWizard();
        }
    }
}