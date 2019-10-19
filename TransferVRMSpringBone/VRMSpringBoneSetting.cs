﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniGLTF;
using VRM;

[CreateAssetMenu( menuName = "VRM/VRMSpringBoneSetting", fileName = "VRMSpringBoneSetting" )]
public class VRMSpringBoneSetting: ScriptableObject
{
    [SerializeField]
    public string m_comment;

    [SerializeField, Range(0, 4), Header("Settings")]
    public float m_stiffnessForce = 1.0f;

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