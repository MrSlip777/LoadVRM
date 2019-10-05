/*
 * DynamicBone、DynamicBoneColliderをオブジェクトから削除するスクリプト
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
    public class RemoveDynamicBone : ScriptableWizard
    {
        static Object[] objects;
        List<string> ColliderTergetName = new List<string>();

        public static void CreateWizard()
        {
            var wiz = ScriptableWizard.DisplayWizard<RemoveDynamicBone>(
                "RemoveDynamicBoneSetting", "remove");
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
            objects = targetObject.GetComponents<DynamicBone>();

            //ボーンに紐付けられているコライダーをピックアップする
            for(int j = 0; j < objects.Length; j++){
                DynamicBone dynamicbone = (DynamicBone)objects[j];

                DynamicBoneSetting exportData
                = ScriptableObject.CreateInstance<DynamicBoneSetting>();
                
                exportData.m_Colliders = new List<string>();
                foreach(DynamicBoneCollider collider in dynamicbone.m_Colliders){
                    exportData.m_Colliders.Add(collider.name);

                    //コライダーのついているオブジェクトの名前を登録する（重複なし、なければ追加）
                    if(ColliderTergetName.Count == 0){
                        ColliderTergetName.Add(collider.name);
                    }
                    else{
                        if(ColliderTergetName.IndexOf(collider.name) == -1){
                            ColliderTergetName.Add(collider.name);
                        };
                    }
                }
            }
        }

        void Remove(){
            for(int j = 0; j<ColliderTergetName.Count; j++){
                GameObject targetObject = GameObject.Find(ColliderTergetName[j]);
                //ダイナミックボーンコライダーを削除
                DynamicBoneCollider[] DynamicBoneComponents = targetObject.GetComponents<DynamicBoneCollider>();
                foreach(DynamicBoneCollider removeComponent in DynamicBoneComponents){
                    DestroyImmediate(removeComponent);
                }
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

    public static class RemoveMenuforDynamicBone
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/DynamicBone/Remove";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void RemoveSettingMenu()
        {
            RemoveDynamicBone.CreateWizard();
        }
    }
}