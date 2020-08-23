using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniGLTF;
using VRM;

[CreateAssetMenu( menuName = "VRM/SpringBoneSetting", fileName = "SpringBoneSetting" )]
public class SpringBoneSetting: ScriptableObject
{
    //付けられているオブジェクト名
    [SerializeField]
    public string m_AttachObject;

    [SerializeField]
    public string m_comment;

    [SerializeField, Range(0f, 5000f), Header("Forces")]
    public float m_stiffnessForce = 0.01f;



    [SerializeField, Range(0, 2)]
    public float m_gravityPower = 0;

    [SerializeField]
    public Vector3 m_gravityDir = new Vector3(0, -1.0f, 0);

    [SerializeField, Range(0, 1)]
    public float m_dragForce = 0.4f;

    [SerializeField]
    public string m_center;

    [SerializeField]
    public string[] RootBones;

    [SerializeField, Range(0, 0.5f), Header("Collider")]
    public float m_hitRadius = 0.02f;

    [SerializeField]
    public string[] ColliderGroups;


}