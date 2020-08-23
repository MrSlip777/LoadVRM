using System;
using UnityEngine;

public class DynamicBoneColliderSetting : ScriptableObject
{
    [SerializeField]
    public string TargetName;

    [Serializable]
    public class SphereCollider
    {
        public DynamicBoneColliderBase.Direction m_Direction = DynamicBoneColliderBase.Direction.Y;
        public Vector3 m_Center = Vector3.zero;
        public DynamicBoneColliderBase.Bound m_Bound = DynamicBoneColliderBase.Bound.Outside;
        public float m_Radius = 0.5f;
        public float m_Height = 0;
    }

    [SerializeField]
    public SphereCollider[] Colliders = new SphereCollider[]{

    };

}