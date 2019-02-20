/*
 * 小物を追加するスクリプト
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
using System.Collections;
using System.IO;

namespace VRM
{

    public class AddOptionObject : ScriptableWizard
    {

        static public Object[] objects;
        static public AddObjectSetting setting;
        static public GameObject parentObject;

        //[SerializeField]
        //Toggle toggle1,toggle2;

        public static void CreateWizard()
        {
            objects = Resources.LoadAll("AttachObjectSetting");
            
        
            var wiz = ScriptableWizard.DisplayWizard<AddOptionObject>(
                "AddOptionObject", "Add");
            var go = Selection.activeObject as GameObject;

        }

        void OnWizardCreate()
        {
            for(int i = 0;  i<objects.Length; i++){
                setting = (AddObjectSetting)objects[i];
                GameObject childObject = Instantiate(setting.Object) as GameObject;
                parentObject = GameObject.Find(setting.parentPartName);
        
                childObject.transform.parent = parentObject.transform;
                childObject.transform.localEulerAngles = setting.rotation;
                childObject.transform.localScale = setting.scale;
                childObject.transform.localPosition = setting.position;
            }
        }

        void OnWizardUpdate()
        {

        }
    }

    public static class VRMExporterMenu
    {
        const string ADD_OPTIONOBJECT_KEY = VRMVersion.VRM_VERSION + "/AddOptionObject";

        [MenuItem(ADD_OPTIONOBJECT_KEY)]
        private static void ExportFromMenu()
        {
            AddOptionObject.CreateWizard();
        }
    }
}
