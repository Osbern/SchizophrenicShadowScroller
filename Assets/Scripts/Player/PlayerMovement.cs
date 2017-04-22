using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Shadow;
    public float Speed;
    public GameObject Caster;
    private bool _isShadowControlled;

    private Rigidbody2D _rigidbody;
    private Animator _playerAnimator;
    private Animator _shadowAnimator;

    //Timer
    private const float INPUT_DELAY = 0.5f;
    private float _inputTimer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _shadowAnimator = Shadow.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _inputTimer += Time.deltaTime;

        if (Input.GetButton("Jump")
            && _inputTimer >= INPUT_DELAY)
        {
            _isShadowControlled = !_isShadowControlled;
            Shadow.transform.localScale = new Vector3(1f, 1f);
            transform.localScale = new Vector3(0.5f, 0.5f);

            if (!_isShadowControlled)
            {
                transform.position = Shadow.transform.position;
                Shadow.transform.localPosition = new Vector3(-0.24f, -0.17f, 0);
            }

            _inputTimer = 0;
        }

        float h = Input.GetAxisRaw("Horizontal");
        Vector2 movement = new Vector2(h, 0);
        movement = movement.normalized * Speed * Time.deltaTime;

        if (_isShadowControlled)
        {
            if (h != 0)
                MoveShadow(movement);
        }
        else
        {
            if (h != 0)
                MoveBoth(movement);

            _playerAnimator.SetFloat("XVelocity", h);
            Caster.transform.position = transform.position;
        }

        _shadowAnimator.SetFloat("XVelocity", h);
    }

    private void MoveBoth(Vector2 movement)
    {
        transform.localScale = (movement.x < 0) ? new Vector3(-0.5f, 0.5f) : new Vector3(0.5f, 0.5f);
        Shadow.transform.localScale = (movement.x < 0) ? new Vector3(-0.5f, 0.5f) : new Vector3(0.5f, 0.5f);

        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        _rigidbody.MovePosition(position + movement);

        //Shadow.transform.position = transform.position + new Vector3(-0.1f, -0.02f, 0);
    }

    private void MoveShadow(Vector2 movement)
    {
        Shadow.transform.localScale = (movement.x < 0) ? new Vector3(-1f, 1f) : new Vector3(1f, 1f);

        Shadow.transform.position += new Vector3(movement.x, movement.y);
    }
}
