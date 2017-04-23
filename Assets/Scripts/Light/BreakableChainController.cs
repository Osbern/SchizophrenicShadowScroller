using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableChainController : MonoBehaviour
{
    public GameObject Light;
    public GameObject RemainChain;

    private Animator _lightAnimator;
    private bool _chainBroken;

    private void Awake()
    {
        _lightAnimator = Light.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1")
            && !_chainBroken)
        {
            _lightAnimator.enabled = true;
            _lightAnimator.SetTrigger("IsBreaking");
            _chainBroken = true;
        }
    }
}
