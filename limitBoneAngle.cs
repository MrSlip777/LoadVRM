using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limitBoneAngle : MonoBehaviour
{
    Vector3 mBoneAngle;
    public float UnderlimitAngle_z;
    public float UpperlimitAngle_z;

    // Update is called once per frame
    void LateUpdate()
    {
        
        mBoneAngle = this.transform.localEulerAngles;
        if(UnderlimitAngle_z < 0){
            if(mBoneAngle.z > UnderlimitAngle_z){
                
                mBoneAngle.z = UnderlimitAngle_z;
                this.transform.localEulerAngles
                = mBoneAngle;
            }
            else{

            }
        }
        else{
            if(mBoneAngle.z < UnderlimitAngle_z){
                
                mBoneAngle.z = UnderlimitAngle_z;
                this.transform.localEulerAngles
                = mBoneAngle;
            }
            else{

            }            
        }

        if(UpperlimitAngle_z < 0){
            if(mBoneAngle.z < UpperlimitAngle_z){
                
                mBoneAngle.z = UpperlimitAngle_z;
                this.transform.localEulerAngles
                = mBoneAngle;
            }
            else{

            }
        }
        else{
            if(mBoneAngle.z > UpperlimitAngle_z){
                
                mBoneAngle.z = UpperlimitAngle_z;
                this.transform.localEulerAngles
                = mBoneAngle;
            }
            else{

            }            
        }

    }
}
