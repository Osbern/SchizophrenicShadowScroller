using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBoxController : MonoBehaviour
{
    private bool _isPizzaPushed;
    private Animator _pizzaBoxAnimator;

    private void Awake()
    {
        _pizzaBoxAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("b")
            && !_isPizzaPushed)
        {
            _pizzaBoxAnimator.SetTrigger("IsPushingBox");
            _isPizzaPushed = true;
        }
    }
}
