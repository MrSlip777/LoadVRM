/*
 * VRMSprinBone設定をDynamicBoneへ反映するスクリプト
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
    public class ReflectSettingSpringBoneToDynamicBone : ScriptableWizard
    {
        static public Object[] boneObjects;
        public List<string> ColliderTergetName = new List<string>();
        static public Object[] colliderObjects;
        public string[] BoneSettingName = null;
        public string[] ColliderSettingName = null;
        
        public static void CreateWizard()
        {
            boneObjects = Resources.LoadAll("SpringBoneData/SpringBone");
            colliderObjects = Resources.LoadAll("SpringBoneData/Collider");

            var wiz = ScriptableWizard.DisplayWizard<ReflectSettingSpringBoneToDynamicBone>(
                "ReflectSettingSpringBoneToDynamicBone", "Apply");
            var go = Selection.activeObject as GameObject;

        }

        void OnWizardCreate()
        {
            //設定ファイルに基づいてSpringBoneColliderを設定する
            ApplyDynamicBoneCollider();
            //設定ファイルに基づいてSpringBoneを設定する
            ApplyDynamicBone();

        }

        void ApplyDynamicBone(){

            //モデル上でsecondaryの部分を探す
            GameObject targetObject = GameObject.Find("secondary");

            //secondaryのスプリングボーン削除
            VRMSpringBone[] SpringBoneComponents = targetObject.GetComponents<VRMSpringBone>();
            foreach(VRMSpringBone removeComponent in SpringBoneComponents){
                DestroyImmediate(removeComponent);
            }

            //secondaryのダイナミックボーン削除
            DynamicBone[] DynamicBoneComponents = targetObject.GetComponents<DynamicBone>();
            foreach(DynamicBone removeComponent in DynamicBoneComponents){
                DestroyImmediate(removeComponent);
            }

            //gizmo以外は設定を反映
            //transformはFind後したTransformを反映)
            for(int j = 0; j < boneObjects.Length; j++){
                //設定データ
                VRMSpringBoneSetting settingData = (VRMSpringBoneSetting)boneObjects[j];

                //settingData.m_comment; 反映しない
                //settingData.m_dragForce;反映しない
                //settingData.m_center 反映しない

                for(int i = 0; i<settingData.RootBones.Length; i++){
                    //対象のボーン
                    DynamicBone dynamicbone = targetObject.AddComponent<DynamicBone>();
                    dynamicbone.m_Colliders = new List<DynamicBoneColliderBase>(); 
                    dynamicbone.m_Stiffness = settingData.m_stiffnessForce * 0.25f;
                    dynamicbone.m_Gravity = settingData.m_gravityDir * settingData.m_gravityPower;
                    GameObject findObject = GameObject.Find(settingData.RootBones[i]);
                    dynamicbone.m_Root = findObject.GetComponent<Transform>();
                    dynamicbone.m_Radius = settingData.m_hitRadius;

                    for(int k = 0; k<settingData.ColliderGroups.Length; k++){
                        findObject = GameObject.Find(settingData.ColliderGroups[k]);
                        dynamicbone.m_Colliders.Add(findObject.GetComponent<DynamicBoneCollider>());
                    }
                }
            }
            
        }

        void ApplyDynamicBoneCollider(){
            //gizmo以外は設定を反映する
            for(int j = 0; j<colliderObjects.Length; j++){
                VRMSpringBoneColliderSetting settingData = (VRMSpringBoneColliderSetting)colliderObjects[j];
                GameObject targetObject = GameObject.Find(settingData.TargetName);

                //スプリングボーンコライダーを削除
                VRMSpringBoneColliderGroup[] SpringBoneComponents = targetObject.GetComponents<VRMSpringBoneColliderGroup>();      
                foreach(VRMSpringBoneColliderGroup removeComponent in SpringBoneComponents){
                    DestroyImmediate(removeComponent);
                }

                //ダイナミックボーンコライダーを削除
                DynamicBoneCollider[] DynamicBoneComponents = targetObject.GetComponents<DynamicBoneCollider>();
                foreach(DynamicBoneCollider removeComponent in DynamicBoneComponents){
                    DestroyImmediate(removeComponent);
                }

                for(int i = 0; i<settingData.Colliders.Length; i++){
                    DynamicBoneCollider collider = targetObject.AddComponent<DynamicBoneCollider>();
                    collider.m_Center = settingData.Colliders[i].Offset;
                    collider.m_Radius = settingData.Colliders[i].Radius;
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
    }

    public static class RelfectDynamicBoneMenu
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/ReflectSettingSpringBoneToDynamicBone";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void RelfectMenu()
        {
           ReflectSettingSpringBoneToDynamicBone.CreateWizard();
        }
    }    
}
