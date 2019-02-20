using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "VRM/ObjectSetting", fileName = "ObjectSetting" )]
public class AddObjectSetting: ScriptableObject
{
    public string parentPartName;
    public GameObject Object;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}
