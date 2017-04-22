using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public GameObject Light;

    private bool _switch;

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _switch = !_switch;
            Light.SetActive(_switch);
        }
    }
}
