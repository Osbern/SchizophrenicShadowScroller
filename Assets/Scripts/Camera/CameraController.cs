using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 movement = new Vector3(h, 0);
        movement = movement.normalized * 5 * Time.deltaTime;
        transform.position += movement;
    }
}
