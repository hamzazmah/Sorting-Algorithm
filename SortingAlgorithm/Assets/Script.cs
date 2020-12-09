using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{

    public GameObject target;//the target object
    public float speedMod = 10.0f;//a speed modifier
    private Vector3 point;//the coord to the point where the camera looks at

    void Start()
    {//Set up things on the start method
        point = target.transform.position;//get target's coords
        transform.LookAt(point);//makes the camera look to it
    }

    // Update is called once per frame
    void Update()
    {
        float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");           
        if (ScrollWheelChange != 0)
        {                                                                              //If the scrollwheel has changed
            float R = ScrollWheelChange * 15;                                          //The radius from current camera
            float PosX = Camera.main.transform.eulerAngles.x + 90;                     //Get up and down
            float PosY = -1 * (Camera.main.transform.eulerAngles.y - 90);              //Get left to right
            PosX = PosX / 180 * Mathf.PI;                                              //Convert from degrees to radians
            PosY = PosY / 180 * Mathf.PI;                                              //^
            float X = R * Mathf.Sin(PosX) * Mathf.Cos(PosY);                           //Calculate new coords
            float Z = R * Mathf.Sin(PosX) * Mathf.Sin(PosY);                           //^
            float Y = R * Mathf.Cos(PosX);                                             //^
            float CamX = Camera.main.transform.position.x;                             //Get current camera postition for the offset
            float CamY = Camera.main.transform.position.y;                             //^
            float CamZ = Camera.main.transform.position.z;                             //^
            Camera.main.transform.position = new Vector3(CamX + X, CamY + Y, CamZ + Z);//Move the main camera
            
        }
  
    }
}