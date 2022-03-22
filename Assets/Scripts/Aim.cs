using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    //[SerializeField] private FieldofView fieldofView;
    Transform aimTransform;
     void Awake() 
    {
        aimTransform = transform.Find("Aim");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = GetMousePosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        //fieldofView.SetAimDirection(aimDirection);
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x)* Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0,0, angle);
    }
    public static Vector3 GetMousePosition()
    {
        Vector3 vec = GetMousePositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMousePositionWithZ()
    {
        return GetMousePositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMousePositionWithZ(Camera worldcamera)
    {
        return GetMousePositionWithZ(Input.mousePosition, worldcamera);
    }
    public static Vector3 GetMousePositionWithZ(Vector3 screenPosition, Camera worldcamera)
    {
        Vector3 worldPosition = worldcamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
