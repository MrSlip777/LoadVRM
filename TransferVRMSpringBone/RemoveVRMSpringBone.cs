/*
 * VRMSpringBone、VRMSpringBoneColliderをオブジェクトから削除するスクリプト
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
    public class RemoveVRMSpringBone : ScriptableWizard
    {
        static Object[] objects;
        List<string> ColliderTergetName = new List<string>();

        public static void CreateWizard()
        {
            var wiz = ScriptableWizard.DisplayWizard<RemoveVRMSpringBone>(
                "RemoveVRMSpringBoneSetting", "remove");
            var go = Selection.activeObject as GameObject;
        }

        void OnWizardCreate()
        {
            //削除対象を探す
            SearchTarget();
            //スプリングボーンとコライダーを削除する
            Remove();
        }

        void SearchTarget(){
            //モデル上でsecondaryの部分を探す
            GameObject targetObject = GameObject.Find("secondary");
            objects = targetObject.GetComponents<VRMSpringBone>();

            //ボーンに紐付けられているコライダーをピックアップする
            for(int j = 0; j < objects.Length; j++){
                VRMSpringBone springbone = (VRMSpringBone)objects[j];

                VRMSpringBoneSetting exportData
                = ScriptableObject.CreateInstance<VRMSpringBoneSetting>();

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
            }
        }

        void Remove(){
            for(int j = 0; j<ColliderTergetName.Count; j++){
                GameObject colliderObject = GameObject.Find(ColliderTergetName[j]);
                GameObject.DestroyImmediate(colliderObject.GetComponent<VRMSpringBoneColliderGroup>());
            }

            //モデル上でsecondaryの部分を探す
            for(int j = 0; j < objects.Length; j++){
                GameObject.DestroyImmediate(objects[j]);
            }

        }
        void OnWizardUpdate(){
            helpString = "設定をExportしてから削除することを推奨します。 It is recommended to export the settings before deleting them.";
 
        }
    }

    public static class RemoveMenu
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/VRMSpringBone/Remove";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void RemoveSettingMenu()
        {
            RemoveVRMSpringBone.CreateWizard();
        }
    }
}