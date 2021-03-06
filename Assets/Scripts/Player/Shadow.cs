﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shadow : Movable
{
    private const float DISAPEARSPEED = 0.02f;
    private const float DISAPEARRATE = 0.001f;

    // Use this for initialization
    void Start()
    {
        Init();
        Child.GetComponent<ParticleSystem>().startSize = 0;
    }

    public override void FootStep()
    {
        // AudioSource.PlayClipAtPoint(FootStepClip, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Enabled)
        {
            Camera.main.GetComponent<FollowCam>().Target = this.gameObject;
            Child.GetComponent<Movable>().Enabled = false;

            if (_rigidbody.velocity.y < -1)
            {
                Disapear();
            }
            else
            {
                _inputTimer += Time.deltaTime;
                _collideTimer += Time.deltaTime;

                if (Input.GetButton("Jump")
                    && _inputTimer >= INPUT_DELAY)
                {
                    Parent.GetComponent<Movable>().Activate(this, true);

                    _inputTimer = 0;
                }

                Move();
            }
        }
    }

    protected override Vector2 Move()
    {
        var movement = base.Move();

        if (movement.y > 0)
        {
            Disapear();
            Child.transform.position = transform.position;
            Child.GetComponent<Movable>().Activate(this);
        }
        else
        {
            //invert direction
            var scale = transform.localScale;
            if ((scale.x > 0 && movement.x < 0) || (scale.x < 0 && movement.x > 0))
            {
                scale.x *= -1;
                transform.localScale = scale;
            }
            Child.transform.localScale = transform.localScale;

            //move
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            _rigidbody.MovePosition(position + new Vector2(movement.x, 0));

            Child.transform.position = transform.position;
        }


        return movement;
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (Enabled)
        {
            if (_collideTimer >= INPUT_DELAY
                && coll.transform.position.y < transform.position.y)
            {
                Disapear();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "LightTrigger")
            SetUnderLight(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "LightTrigger")
            SetUnderLight(false);
    }

    public void SetUnderLight(bool isUnderLight)
    {
        foreach (var particle in GetComponentsInChildren<ParticleSystem>())
        {
            if (isUnderLight)
            {
                particle.Play();
            }
            else
            {
                particle.Stop();
            }
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (Enabled)
        {
            if ((LayerMask.LayerToName(coll.gameObject.layer) == "Ground"))
            {
                _collideTimer = 0;
            }
        }
    }
    private void Disapear()
    {
        if (!_isDisapearing && !_apear)
        {
            _isDisapearing = true;
            Child.GetComponent<Movable>().Activate(this);
            InvokeRepeating("DisapearMe", 0, DISAPEARRATE);
        }
    }

    private bool _isDisapearing, _apear = false;

    private void DisapearMe()
    {
        var scale = transform.localScale;
        var newVal = Mathf.Clamp(scale.x - DISAPEARSPEED, 0, 1);
        transform.localScale = new Vector3(newVal, newVal);

        if (transform.localScale.x <= 0)
        {
            SetUnderLight(false);
            _isDisapearing = false;
            SetUnderLight(false);
            CancelInvoke("DisapearMe");
        }
    }

    public void Apear(bool ApearParent = false)
    {
        if (ApearParent)
        {
            Parent.transform.position = transform.position;
            Parent.GetComponent<Movable>().Activate(this);
        }
        if (!_isDisapearing && !_apear)
        {
            SetUnderLight(true);
            _apear = true;
            InvokeRepeating("ApearMe", 0, DISAPEARRATE);
        }

    }

    private void ApearMe()
    {
        var scale = transform.localScale;
        var newVal = Mathf.Clamp(scale.x + DISAPEARSPEED, 0, 1);
        transform.localScale = new Vector3(newVal, newVal);

        if (newVal >= 1)
        {
            Enabled = true;
            _apear = false;
            CancelInvoke("ApearMe");
        }
    }
}
