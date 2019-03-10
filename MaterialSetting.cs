using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "VRM/MaterialSetting", fileName = "MaterialSetting" )]
public class MaterialSetting: ScriptableObject
{
    public string targetName;
    public Material[] mat = new Material[10];
}