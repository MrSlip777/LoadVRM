/*
 * SpringBone、SpringBoneColliderをオブジェクトから削除するスクリプト
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
    public class RemoveSpringBoneWizard : ScriptableWizard
    {
        //対象になるモデル
        public GameObject targetModel;

        static Object[] objects;
        List<string> ColliderTergetName = new List<string>();

        private static GameObject m_Wizard;
        private static RemoveSpringBone m_RemoveSpringBone = null;

        public static void CreateWizard()
        {
            var wiz = ScriptableWizard.DisplayWizard<RemoveSpringBoneWizard>("RemoveSpringBoneSetting", "remove");
            var go = Selection.activeObject as GameObject;

            m_Wizard = new GameObject();
            m_RemoveSpringBone = m_Wizard.AddComponent<RemoveSpringBone>();

        }

        void OnWizardCreate()
        {
            //ボーンとコライダーを削除する
            m_RemoveSpringBone.Remove(targetModel);
        }

        void OnWizardUpdate(){
            helpString = "設定をExportしてから削除することを推奨します。 It is recommended to export the settings before deleting them.";
 
        }

        void OnDestroy(){
            DestroyImmediate(m_Wizard);
        }         
    }

    public class RemoveSpringBone : MonoBehaviour
    {
        public void Remove(GameObject model){
            //モデル上のボーンを探す
            SpringBone[] bones = model.GetComponentsInChildren<SpringBone>();

            foreach(SpringBone bone in bones){
                DestroyImmediate(bone);
            }

            SpringBoneColliderGroup[] colliders = model.GetComponentsInChildren<SpringBoneColliderGroup>();

            foreach(SpringBoneColliderGroup collider in colliders){
                DestroyImmediate(collider);
            }

        }
    }

    public static class RemoveMenu
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/SpringBone/Remove";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void RemoveSettingMenu()
        {
            RemoveSpringBoneWizard.CreateWizard();
        }
    }
}

#endif