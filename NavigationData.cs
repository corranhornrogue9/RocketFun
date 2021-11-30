using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationData : MonoBehaviour
{
    Rigidbody _rigidbody;
    private Vector3 targetPos;
    private Vector3 course;
    private float fidelty;
    private bool breaking;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void LogDebug()
    {
        Debug.Log("DTT = " + DistanceToTarget + ", Speed = " + CurrentSpeed + ", Course Accuracy" + CourseAccuracy + ", VMG = " + VMG + ", Breaking = " + Breaking);
    }

    public void UpdateNavTarget(Vector3 newTarget)
    {
        targetPos = newTarget;
    }

    public void UpdateNavCourse(Vector3 course)
    {
        this.course = course;
    }

    public void UpdateFidelty(float fidelty)
    {
        this.fidelty = fidelty;
    }

    public void UpdateBreaking(bool breaking)
    {
        this.breaking = breaking;
    }
    public Vector3 VectorToTarget
    {
        get
        {
            return targetPos - transform.position;
        }
    }

    public float DistanceToTarget
    {

        get
        {
            return VectorToTarget.magnitude;
        }
    }

    public float CurrentSpeed
    {

        get
        {
            if (_rigidbody != null)
            {
                return _rigidbody.velocity.magnitude;
            }
            else
            {
                return 0;
            }
        }
    }

    public Vector3 CurrentVector
    {
        get
        {
            if (_rigidbody != null)
            {
                return _rigidbody.velocity;
            }
            else
            {
                return new Vector3();
            }
        }
    }

    public Vector3 Course
    {
        get
        {
            return course;
        }
    }

    public float CourseAccuracy
    {
        get
        {
            return fidelty;
        }
    }

    public float VMG
    {
        get
        {
            if (_rigidbody != null)
            {
                return Vector3.Project(_rigidbody.velocity, Course).magnitude;
            }
            else
            {
                return 0;
            }
        }
    }

    public bool Breaking
    {
        get
        {
            return breaking;
        }
    }
}
