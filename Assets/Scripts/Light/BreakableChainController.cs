using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableChainController : Trigger
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
        if (_playerIn != null && Input.GetButton("Fire1") && !_chainBroken)
        {
            _playerIn.GetComponent<Animator>().Play("Action");
            _lightAnimator.enabled = true;
            _lightAnimator.SetTrigger("IsBreaking");
            _chainBroken = true;
        }
    }
}
