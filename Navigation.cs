using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    // Start is called before the first frame update
    SimpleSeeker seeker;
    RocketMotor motor;
    public float manouvreFidelty = 1f;
    Rigidbody _rigidbody;

    public float mavovrabilty = 5;
    float boostFuel;

    public float NEZ = 250;
    public float wayPointSize = 250;

    public List<NavigationPoint> navpoints;

    public NavigationData navData;

    private RangeCoefficent AccuracyCoefficent;

    private static RangeCoefficent defaultCoefficnet;

    void Start()
    {
         if(AccuracyCoefficent == null)
        {
            DefaultCoefficent();
        }
        seeker = GetComponent<SimpleSeeker>();
        motor = GetComponent<RocketMotor>();
        _rigidbody = GetComponent<Rigidbody>();
        navData = this.transform.gameObject.AddComponent<NavigationData>();
       
    }

    private void DefaultCoefficent()
    {
        if(defaultCoefficnet == null)
        {
            defaultCoefficnet = new RangeCoefficent();
            defaultCoefficnet.AddNode(0.1f, 5000, 25);
            defaultCoefficnet.AddNode(1f, 5000, 15);
            defaultCoefficnet.AddNode(10f, 5000, 15);
            defaultCoefficnet.AddNode(100f, 5000, 10);
            defaultCoefficnet.AddNode(1000f, 5000, 3);
            defaultCoefficnet.AddNode(0.1f, 1000, 20);
            defaultCoefficnet.AddNode(1f, 1000, 15);
            defaultCoefficnet.AddNode(10f, 1000, 10);
            defaultCoefficnet.AddNode(100f, 1000, 8);
            defaultCoefficnet.AddNode(1000f, 1000, 3);
            defaultCoefficnet.AddNode(0.1f, 250, 5);
            defaultCoefficnet.AddNode(1f, 250, 4);
            defaultCoefficnet.AddNode(10f, 250, 3);
            defaultCoefficnet.AddNode(100f, 250, 2);
            defaultCoefficnet.AddNode(1000f, 250, 1);
            defaultCoefficnet.AddNode(0.1f, 50, 3);
            defaultCoefficnet.AddNode(1f, 50, 2.5f);
            defaultCoefficnet.AddNode(10f, 50, 2);
            defaultCoefficnet.AddNode(100f, 50, 1);
            defaultCoefficnet.AddNode(1000f, 50, 0.1f);
        }

        AccuracyCoefficent = defaultCoefficnet;
    }

    public void Boost(float time)
    {
        boostFuel = time;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
      
            MaxThrustAdjust();
       
    }

   

    
    private float TopSpeed()
    {
        var addedSpeed = (motor.MaxThrust * boostFuel);
        var topSpeed = _rigidbody.velocity.magnitude + addedSpeed;
        return topSpeed;
    }

    private float PredictedSpeed(Vector3 intendedDirection)
    {
        var addedSpeed = (motor.MaxThrust * boostFuel);
        var topSpeed = Vector3.Project(_rigidbody.velocity, intendedDirection).magnitude + addedSpeed;
        return topSpeed;
    }

    private void MaxThrustAdjust()
    {
        var navPoint = navpoints.First();

        navData.UpdateNavTarget(navPoint.transform.position);

        if (navPoint.NavType == NavigationPoint.Type.Waypoint)
        {
            navPoint = GetNextWaypoint(navPoint);
        }

        boostFuel += navPoint.GetBoost();


        var vectorToIntercept = DirectionToInterceptTarget(navPoint.transform, navData.VectorToTarget);

        navData.UpdateBreaking(false);
        if (navPoint.NavType == NavigationPoint.Type.Endpoint)
        {
            vectorToIntercept = CalculateTurnpoint(navPoint, vectorToIntercept);
        }

        navData.UpdateNavCourse(vectorToIntercept);
        
        navData.UpdateFidelty(CalculateAccurace());

        if (Vector3.Angle(navData.Course, navData.CurrentVector) > navData.CourseAccuracy)
        {
            OffCourseCorrect();
        }
        else
        {
            OnCourseAccelerateIfPossible();
        }

        if (NEZ > navData.DistanceToTarget)
        {
            motor.TurnOn();
        }

    }

    private void OnCourseAccelerateIfPossible()
    {
        seeker.SetTargetRotation(navData.Course);
        if (boostFuel > 0)
        {
            boostFuel -= Time.fixedDeltaTime;

            if (boostFuel < 0)
            {
                boostFuel = 0;
            }

            motor.TurnOn();
        }
        else
        {
            motor.TurnOff();
        }
    }

    private void OffCourseCorrect()
    {
        var correction = (navData.Course.normalized) - (navData.CurrentVector.normalized);

        seeker.SetTargetRotation(correction);
        ThrustIfCloseToRotateTarget(correction);
    }

    private void ThrustIfCloseToRotateTarget(Vector3 correction)
    {
        if (Vector3.Angle(correction, transform.forward) < manouvreFidelty)
        {
            motor.TurnOn();
        }
        else
        {

            motor.TurnOff();
        }
    }

    private float CalculateAccurace()
    {
        float accuracy = 0;



        accuracy = AccuracyCoefficent.Accuracy(navData.DistanceToTarget, navData.VMG, navData.Breaking);
        return accuracy *= 1;
    }


    private NavigationPoint GetNextWaypoint(NavigationPoint navPoint)
    {
        if (wayPointSize > navData.DistanceToTarget)
        {

            navpoints.Remove(navPoint);
            navPoint = navpoints.First();
        }

        return navPoint;
    }

    private Vector3 CalculateTurnpoint(NavigationPoint navPoint, Vector3 vectorToIntercept)
    {
        var stopDistance = (0.5 * navData.CurrentSpeed) * (navData.CurrentSpeed / motor.MaxThrust);
        boostFuel = 1;
        // add the time to turn around
        stopDistance += seeker.TimeToTargetRoate(-this.transform.forward) * navData.CurrentSpeed;
        stopDistance *= navPoint.safteyMargin;

        if (stopDistance > navData.DistanceToTarget)
        {
            vectorToIntercept = -vectorToIntercept;
            navData.UpdateBreaking(true);
        }
        

        return vectorToIntercept;
    }

    /// <summary>
    /// <para>Assume speed is constant no need to calculate relative speed of laser to get interception pos!</para>
    /// <para>Calculates interception point between two moving objects where chaser speed is known but chaser vector is not known(Angle to fire at * speed"*Sort of*")</para>
    /// <para>Can use System.Math and doubles to make this formula NASA like precision.</para>
    /// </summary>
    /// <param name="PC">interceptor position</param>
    /// <param name="SC">Speed of laser</param>
    /// <param name="PR">Target initial position</param>
    /// <param name="VR">Target velocity vector</param>
    /// <returns>Interception Point as World Position</returns>
    private Vector3 CalculateInterceptionPoint3D(Vector3 PC, float SC, Vector3 PR, Vector3 VR)
    {
        //! Distance between turret and target
        Vector3 D = PC - PR;

        //! Scale of distance vector
        float d = D.magnitude;

        //! Speed of target scale of VR
        float SR = VR.magnitude;

        //% Quadratic EQUATION members = (ax)^2 + bx + c = 0

        float a = Mathf.Pow(SC, 2) - Mathf.Pow(SR, 2);

        float b = 2 * Vector3.Dot(D, VR);

        float c = -Vector3.Dot(D, D);

        if ((Mathf.Pow(b, 2) - (4 * (a * c))) < 0) //% The QUADRATIC FORMULA will not return a real number because sqrt(-value) is not a real number thus no interception
        {
            return Vector2.zero;
        }
        //% Quadratic FORMULA = x = (  -b+sqrt( ((b)^2) * 4*a*c )  ) / 2a
        float t = (-(b) + Mathf.Sqrt(Mathf.Pow(b, 2) - (4 * (a * c)))) / (2 * a);//% x = time to reach interception point which is = t

        //% Calculate point of interception as vector from calculating distance between target and interception by t * VelocityVector
        return ((t * VR) + PR);
    }

    private Vector3 DirectionToInterceptTarget(Transform targetTransform, Vector3 vectorToTarget)
    {
        Vector3 retval;
        if (targetTransform.gameObject.TryGetComponent<Rigidbody>(out _))
        {

            var interceptPoint = CalculateInterceptionPoint3D(transform.position, PredictedSpeed(vectorToTarget), targetTransform.position, targetTransform.gameObject.GetComponent<Rigidbody>().velocity);
            retval = interceptPoint - transform.position;

        }
        else
        {
            retval = vectorToTarget;
        }

        return retval;
    }
    /*

      if (AccuracyCoefficent.Count< 1)
        {
            accuracy = 1;
        }
        else if (AccuracyCoefficent.Count == 1)
{
    accuracy = AccuracyCoefficent[0].Item2;
}
else if (AccuracyCoefficent[AccuracyCoefficent.Count - 1].Item1 > navData.DistanceToTarget)
{
    accuracy = AccuracyCoefficent[AccuracyCoefficent.Count - 1].Item2;
}
else if (AccuracyCoefficent[0].Item1 < navData.DistanceToTarget)
{
    accuracy = AccuracyCoefficent[0].Item2;
}
else
{
    for (int i = 0; i < AccuracyCoefficent.Count; i++)
    {
        if (AccuracyCoefficent[i].Item1 > navData.DistanceToTarget && AccuracyCoefficent[i + 1].Item1 < navData.DistanceToTarget)
        {
            accuracy = InterpolateAccuracy(AccuracyCoefficent[i], AccuracyCoefficent[i + 1], navData.DistanceToTarget);
        }
    }
    */

}
