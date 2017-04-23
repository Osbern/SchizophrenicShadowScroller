using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h == 0)
            return;

        Vector3 movement = new Vector3(h, 0);
        movement = movement.normalized * 20 * Time.deltaTime;

        float newX = transform.position.x + movement.x;

        if (newX > -19f
            && newX < 98f)
            transform.position += movement;
    }
}
