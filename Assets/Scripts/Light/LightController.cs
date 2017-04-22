using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public GameObject Light;
    public Animator LightAnimator;

    private bool _switch;

    private void Awake()
    {
        if (LightAnimator != null)
            LightAnimator.enabled = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _switch = !_switch;
            Light.SetActive(_switch);
        }
    }
}
