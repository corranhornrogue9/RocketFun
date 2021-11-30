using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Display : MonoBehaviour
{
    public List<GameObject> trackedObjects;
    public GameObject trackerSymbol;
    private List<Tuple<GameObject, GameObject>> trackers;
    private float scale;
    // Start is called before the first frame update
    void Start()
    {
        trackers = new List<Tuple<GameObject, GameObject>>();
        foreach(var go in trackedObjects)
        {
            var trackerSymbolGO = GameObject.Instantiate(trackerSymbol);
            trackerSymbolGO.transform.parent = this.transform;
            var match = new Tuple<GameObject, GameObject>(go, trackerSymbolGO);
            trackers.Add(match);

            NavigationPoint poit;
            if(go.TryGetComponent<NavigationPoint>(out poit))
            {
                if(poit.NavType == NavigationPoint.Type.Endpoint)
                {
                    trackerSymbolGO.GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
                else if (poit.NavType == NavigationPoint.Type.Target)
                {
                    trackerSymbolGO.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else if (poit.NavType == NavigationPoint.Type.Waypoint)
                {
                    trackerSymbolGO.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                else
                {
                    trackerSymbolGO.GetComponent<MeshRenderer>().material.color = Color.magenta;
                }
            }
            else
            {
                trackerSymbolGO.GetComponent<MeshRenderer>().material.color = Color.blue;
            }
        }

        scale = 1 / (trackedObjects.Select(a => (a.transform.position - this.transform.position).magnitude).OrderBy(a => a).First() * 5000 );

    }

    // Update is called once per frame
    void Update()
    {
        var currentPos = this.transform.position;
        
        foreach (var go in trackers)
        {
            go.Item2.transform.rotation = go.Item1.transform.rotation;
            var relativePos = go.Item1.transform.position - currentPos;
            relativePos *= scale;
            go.Item2.transform.localPosition = relativePos;

        }
    }
}
