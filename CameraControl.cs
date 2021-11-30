using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Display display;
    int currerntDisplay = 0;
    public float lookDistnace = 0.5f;
    public float lookDistanceResoultion = 1f;
    public float rotateSpeed = 1f;

    GameObject trackedGameObject;
    // Start is called before the first frame update
    void Start()
    {
        display = this.GetComponentInChildren<Display>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if(trackedGameObject != null)
        {
            transform.position = trackedGameObject.transform.position - transform.forward * lookDistnace;
            NavigationData data;
            if(trackedGameObject.TryGetComponent<NavigationData>(out data))
            {
                data.LogDebug();
            }

        }

        if(Input.GetMouseButton(1))
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * -1, Input.GetAxis("Mouse X") * -1, 0) * Time.deltaTime * rotateSpeed);
      
        }



      
        lookDistnace += (Input.GetAxis("Mouse ScrollWheel") * -1) * lookDistanceResoultion;
        lookDistnace = Mathf.Clamp(lookDistnace, 0.5f, 50);

        var numberKeyVlaue = GetPressedNumber();
      
        if(numberKeyVlaue > -1)
        {
            if(numberKeyVlaue == 0)
            {
                numberKeyVlaue = 10;
            }
            GetNextDisplay(numberKeyVlaue -1);
        }

        if(Input.GetKeyDown(KeyCode.PageUp))
        {
            GetNextDisplay();
        }
        if(Input.GetKeyDown(KeyCode.PageDown))
        {
            GetNextDisplay(true);
        }
    }

    void GetNextDisplay(bool backwards = false)
    {
        int newDisplayIncrease = 1;
        if(backwards)
        {
            newDisplayIncrease = -1;
        }

        if(display.trackedObjects.Count <= 1 || display.trackedObjects.Count - 1 < currerntDisplay + newDisplayIncrease)
        {
            currerntDisplay = 0;
        }
        else if (currerntDisplay + newDisplayIncrease < 0)
        {
            currerntDisplay = display.trackedObjects.Count - 1;
        }
        else
        {
            currerntDisplay += newDisplayIncrease;
        }

        Debug.Log("changing to display object " + currerntDisplay);

        trackedGameObject = display.trackedObjects[currerntDisplay];
    }
    void  GetNextDisplay(int postion)
    {
        if(postion < display.trackedObjects.Count && postion >= 0)
        {
            trackedGameObject = display.trackedObjects[postion];
            currerntDisplay = postion;
            Debug.Log("changing to display object " + currerntDisplay);
        }
    }

    public int GetPressedNumber()
    {
        for (int number = 0; number <= 9; number++)
        {
            if (Input.GetKeyDown(number.ToString()))
                return number;
        }

        return -1;
    }
}
