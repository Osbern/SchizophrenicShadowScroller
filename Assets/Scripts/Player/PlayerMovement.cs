using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public GameObject Caster;

    private Vector2 _movement;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        Move(h);
        Caster.transform.position = transform.position;
    }

    private void Move(float h)
    {
        _movement.Set(h, 0);
        _movement = _movement.normalized * Speed * Time.deltaTime;

        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        _rigidbody.MovePosition(position + _movement);

        _animator.SetFloat("XVelocity", h);
    }
}
