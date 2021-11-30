using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSeeker : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
       
    }

    public void SetTargetRotation(Vector3 newtarget)
    {
        targetRotation = newtarget;
    }

    // Update is called once per frame
    public Vector3 targetRotation;

    // Angular speed in radians per sec.
    public float rotateForce = 0.10f;
    public float stopThreshold = 0.5f;

    void FixedUpdate()
    {
        if (targetRotation != null)
        {
            
            var rotationSpeed = m_Rigidbody.angularVelocity;
            Vector3 cross = Vector3.Cross(transform.forward, targetRotation);

            // trying to get angluer stoping distnace based on torque to be converted to how far it will rotate before stoipping.  distnace  = 0.5 * ((mass * velocity^2) / force)
            ///var stopDistanceX = 0.5 * ((m_Rigidbody.inertiaTensor.x * Mathf.Pow(rotationSpeed.x, 2)) / rotateForce);
       

            var stopDistanceX = (0.5 * rotationSpeed.x) * (rotationSpeed.x / rotateForce); 
            
            var stopDistanceY = (0.5 * rotationSpeed.y) * (rotationSpeed.y / rotateForce);
            var stopDistanceZ = (0.5 * rotationSpeed.z) * (rotationSpeed.z / rotateForce);

            var rot = Quaternion.FromToRotation(transform.forward, targetRotation).eulerAngles;


            var rotx = rot.x;
            if(rotx > 180)
            {
                rotx -= 360;
                rotx *= -1;
            }

            var roty = rot.y;
            if (roty > 180)
            {
                roty -= 360;
                roty *= -1;
            }

            var rotz = rot.z;
            if (rotz > 180)
            {
                rotz -= 360;
                rotz *= -1;
            }


            var rotateVector = NormalizeVector(cross);

            if ((rotateVector.x > 0 && rotationSpeed.x > 0 || (rotateVector.x < 0 && rotationSpeed.x < 0)) && (Mathf.Deg2Rad * rotx) < stopDistanceX)
            {
                rotateVector.x *= -1;
            }
            if ((rotateVector.y > 0 && rotationSpeed.y > 0 || (rotateVector.y < 0 && rotationSpeed.y < 0)) && (Mathf.Deg2Rad * roty) < stopDistanceY)
            {
                
                rotateVector.y *= -1;
            }
            if ((rotateVector.z > 0 && rotationSpeed.z > 0 || (rotateVector.z < 0 && rotationSpeed.z < 0)) && (Mathf.Deg2Rad * rotz) < stopDistanceZ)
            {
                rotateVector.z *= -1;
            }

            // apply torque along that axis according to the magnitude of the angle.
            m_Rigidbody.AddTorque(rotateVector * rotateForce, ForceMode.Acceleration);
            

        }
    }

    private Vector3 NormalizeVector(Vector3 vect)
    {
        return new Vector3(NormalizeFloat(vect.x), NormalizeFloat(vect.y), NormalizeFloat(vect.z));
    }

    private float NormalizeFloat(float fl)
    {
        if(fl > 0)
        {
            fl = 1;
        }
        if(fl < 0)
        {
            fl = -1;
        }

        return fl;
    }

    public float TimeToTargetRoate()
    {
        return TimeToTargetRoate(targetRotation);

    }

    public float TimeToTargetRoate(Vector3 target)
    {
        var rot = Quaternion.FromToRotation(transform.forward, target).eulerAngles;

        var biggistTurn = Mathf.Max(new float[] { rot.x, rot.y, rot.z });

        var brot = biggistTurn;
        if (brot > 180)
        {
            brot -= 360;
            brot *= -1;
        }

        brot *= Mathf.Deg2Rad;


        var aveargeTurnSpeed = Mathf.Sqrt((2 * rotateForce) * (brot / 2)) / 2;
        return brot / aveargeTurnSpeed;

    }
}
