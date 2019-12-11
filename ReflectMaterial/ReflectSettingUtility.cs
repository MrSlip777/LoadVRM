using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectSettingUtility : MonoBehaviour
{
    public string GetHierarchyPath(Transform self)
    {
        string path = self.gameObject.name;
        Transform parent = self.parent;
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }
        return path;
    }
}
