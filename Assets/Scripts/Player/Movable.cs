﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    public GameObject Child;
    public GameObject Parent;

    public float Speed;
    protected Rigidbody2D _rigidbody;
    protected Animator _animator, _childAnimator;
    public bool Enabled = false;
    protected bool GoToChild = false;

    protected float _inputTimer;

    protected const float INPUT_DELAY = 0.5f;

    // Use this for initialization
    void Start()
    {

    }

    protected void Init()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        if (Child != null)
        {
            _childAnimator = Child.GetComponent<Animator>();
        }

    }
    public void Activate(Movable caller, bool goToChild = false)
    {
        if (this.GetType() == typeof(ShadowBall))
        {
            transform.position = caller.transform.position;
            transform.localScale = Vector3.one;
            gameObject.GetComponent<ParticleSystem>().Play();
        }
        _inputTimer = 0;
        caller.Enabled = false;
        GoToChild = goToChild;
        this.Enabled = true;
    }

    protected float GetH()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    protected float GetV()
    {
        return Input.GetAxisRaw("Vertical");
    }

    protected virtual Vector2 Move()
    {
        Vector2 movement = new Vector2(GetH(), GetV());
        movement = movement.normalized * Speed * Time.deltaTime;

        if (_animator != null)
        {
            _animator.SetFloat("XVelocity", movement.x);
        }

        if (_childAnimator != null)
        {
            _childAnimator.SetFloat("XVelocity", movement.x);
        }

        return movement;
    }

    // Update is called once per frame
    void Update()
    {


    }
}