using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    //TODO this should be using the new input system
    Rigidbody rig;

    public UnityEvent ShootEvent;

    public float rotationSpeed = 50;

    public float speed = 10;
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAtMouse();

        if (Input.GetKey(KeyCode.W))
        {
            rig.velocity = speed * Time.deltaTime * transform.forward;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootEvent.Invoke();
        }
    }

    // Rotate around the y axis to point towards the mouse

    
    // rotate the object so it points towards the mouse

    void LookAtMouse()
    {
        // Generate a plane that intersects the transform's position with an upwards normal.
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        // Generate a ray from the cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Determine the point where the cursor ray intersects the plane.
        // If the ray is parallel to the plane, Raycast will return false.
        if (playerPlane.Raycast(ray, out float hitdist))
        {
            // Get the point along the ray that hits the calculated distance.
            Vector3 targetPoint = ray.GetPoint(hitdist);
            // Determine the target rotation.  This is the rotation if the transform looks at the target point.
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
