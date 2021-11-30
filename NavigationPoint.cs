using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NavigationPoint : MonoBehaviour
{
    public enum Type
    {
        Target =1,
        Waypoint = 2,
        Endpoint = 3,

    }

    public float safteyMargin = 1.1f;

    public float boost = 3;

    public Type NavType
    {
        get
        {
            var navType = Type.Waypoint;
            if (typeName.ToLower().Equals("target"))
            {
                navType = Type.Target;
            }
            if (typeName.ToLower().Equals("endpoint"))
            {
                navType = Type.Endpoint;
            }

            return navType;
        }
    }

    public string typeName = "waypoint";
   
    // Start is called before the first frame update
    void Start()
    {
        

    }

    public float GetBoost()
    {
        var b = boost;
        boost = 0;
        
        return b;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
