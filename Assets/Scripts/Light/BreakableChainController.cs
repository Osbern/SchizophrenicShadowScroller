using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableChainController : MonoBehaviour
{
    public GameObject Light;
    public GameObject RemainChain;

    private bool _chainBroken;
    private bool _chainDisabled;
    private float _angle;

    private void Update()
    {
        if (Input.GetKeyDown("c")
            && !_chainDisabled)
        {
            _chainBroken = true;
        }

        if (_chainBroken)
        {
            _angle += Time.deltaTime;

            Light.transform.RotateAround(RemainChain.transform.position, new Vector3(0, 0, 1), _angle);

            if (_angle >= 1.2f)
            {
                _chainBroken = false;
                _chainDisabled = true;
            }
        }
    }
}
