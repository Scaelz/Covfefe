using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float min_x, max_x, lerp_speed;
    private Vector3 dragOrigin;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3( pos.x * lerp_speed, 0, 0);//Mathf.Clamp(pos.x * lerp_speed, min_x, max_x)

        transform.Translate(move, Space.World);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min_x, max_x), 
                                         transform.position.y, transform.position.z);


        Debug.Log(move);
    }
}
