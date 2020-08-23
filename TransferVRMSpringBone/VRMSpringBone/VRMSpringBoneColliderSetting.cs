using System;
using UnityEngine;

public class VRMSpringBoneColliderSetting : ScriptableObject
{
    [SerializeField]
    public string TargetName;

    [Serializable]
    public class SphereCollider
    {
        public Vector3 Offset;

        [Range(0, 1.0f)]
        public float Radius;
    }

    [SerializeField]
    public SphereCollider[] Colliders = new SphereCollider[]{
        new SphereCollider
        {
            Radius=0.1f
        }
    };
}
