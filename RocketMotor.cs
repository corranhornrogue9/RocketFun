using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMotor : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    private float m_Thrust = 0f;
    public float MaxThrust = 20f;
    public float intialBurn = 0;
    public float fuel = 10;
    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.AddForce(transform.forward * intialBurn, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOn()
    {
        m_Thrust = MaxThrust;
    }

    public void TurnOff()
    {
        m_Thrust = 0;
    }

    private void FixedUpdate()
    {
        //Apply a force to this Rigidbody in direction of this GameObjects up axis


        if (fuel > 0 && m_Thrust > 0.01)
        {
            fuel -= Time.fixedDeltaTime;
            m_Rigidbody.AddForce(transform.forward * m_Thrust, ForceMode.Acceleration);
          
        }

      
    }
}
