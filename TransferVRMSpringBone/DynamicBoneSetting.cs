using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniGLTF;

[CreateAssetMenu( menuName = "VRM/DynamicBoneSetting", fileName = "DynamicBoneSetting" )]
public class DynamicBoneSetting: ScriptableObject
{
    [SerializeField]
    public string m_Root;

    [SerializeField]
    public float m_UpdateRate = 60;
    
    [SerializeField]
    public DynamicBone.UpdateMode m_UpdateMode = DynamicBone.UpdateMode.Normal;
    
    [SerializeField,Range(0, 1)]
    public float m_Damping = 0.1f;
    
    [SerializeField]
    public AnimationCurve m_DampingDistrib = null;

	[SerializeField,Range(0, 1)]
    public float m_Elasticity = 0.1f;
    
    [SerializeField]
    public AnimationCurve m_ElasticityDistrib = null;
    
    [SerializeField,Range(0, 1)]
    public float m_Stiffness = 0.1f;
    
    [SerializeField]
    public AnimationCurve m_StiffnessDistrib = null;
	
    [SerializeField,Range(0, 1)]
    public float m_Inert = 0;
    
    [SerializeField]
    public AnimationCurve m_InertDistrib = null;
	
    [SerializeField]
    public float m_Radius = 0;
    
    [SerializeField]
    public AnimationCurve m_RadiusDistrib = null;

    [SerializeField]
    public float m_EndLength = 0;
    
    [SerializeField]
    public Vector3 m_EndOffset = Vector3.zero;
	
    [SerializeField]
    public Vector3 m_Gravity = Vector3.zero;
	
    [SerializeField]
    public Vector3 m_Force = Vector3.zero;
	
    [SerializeField]
    public List<string> m_Colliders = null;
	
    [SerializeField]
    public List<string> m_Exclusions = null;
	
    [SerializeField]
    public DynamicBone.FreezeAxis m_FreezeAxis;
	
    [SerializeField]
    public bool m_DistantDisable = false;
    
    [SerializeField]
    public string m_ReferenceObject;
    
    [SerializeField]
    public float m_DistanceToObject = 20;
}