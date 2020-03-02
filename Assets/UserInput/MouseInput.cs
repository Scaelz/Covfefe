using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            OnHoldHandler();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnUpHandler();
        }
    }

    

    public void OnHoldHandler()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            IClickable clickable = hit.transform.GetComponent<IClickable>();
            clickable?.OnHold();
            // Do something with the object that was hit by the raycast.
        }
    }

    public void OnUpHandler()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            IClickable clickable = hit.transform.GetComponent<IClickable>();
            clickable?.OnUp();
            // Do something with the object that was hit by the raycast.
        }
    }
}
