using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : Trigger
{
    public GameObject Light;
    public Animator LightAnimator;

    public bool _switch = true;

    void Start()
    {
        Light.SetActive(_switch);
    }

    private void Awake()
    {
        if (LightAnimator != null)
            LightAnimator.enabled = false;
    }

    private void Update()
    {
        if (_playerIn != null && Input.GetButton("Fire1"))
        {
            _playerIn.GetComponent<Animator>().Play("Action");
            _switch = !_switch;
            Light.SetActive(_switch);
        }
    }
}
