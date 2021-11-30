using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIdicator : MonoBehaviour
{
    public GameObject directionIndicator;
    // Start is called before the first frame update
    Rigidbody m_RigidBody;
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        directionIndicator.transform.rotation = Quaternion.LookRotation(  m_RigidBody.velocity, directionIndicator.transform.up);
    }
}
