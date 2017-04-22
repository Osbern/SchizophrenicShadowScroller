using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private GameObject _shadowBall;
    //Timer
    private const float INPUT_DELAY = 0.5f;
    private const float DISAPEARSPEED = 0.02f;
    private const float DISAPEARRATE = 0.001f;
    private const float MAXSHADOWSIZE = 0.5f;
    private float _inputTimer;

    public bool IsActiveShadowBall, IsUnActiveShadowBall = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _shadowAnimator = Shadow.GetComponent<Animator>();
        _shadowBall = GetComponentsInChildren<Transform>().First(x => x.name == "shadowball").gameObject;
        var particleColor = _shadowBall.GetComponent<ParticleSystem>().startColor;
        _shadowBall.GetComponent<ParticleSystem>().startSize = 0;
        _shadowBall.SetActive(false);
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
        var v = Input.GetAxis("Vertical");

        //ball or shadow
        var activeShadowBall = v > 0;
        if (_isShadowControlled && !_shadowBall.activeInHierarchy && activeShadowBall)
        {
            _shadowBall.transform.position = Shadow.transform.position;
            if (!IsActiveShadowBall && !IsUnActiveShadowBall)
            {
                InvokeRepeating("ActiveShadowBall", 0, DISAPEARRATE);
            }

        }

        Vector2 movement = new Vector2(h, _shadowBall.activeInHierarchy ? v : 0);
        movement = movement.normalized * Speed * Time.deltaTime * (_shadowBall.activeInHierarchy ? 2 : 1);

        if (_isShadowControlled)
        {
            _shadowBall.SetActive(!Shadow.activeInHierarchy);

            if (h != 0 || v != 0)
                MoveShadow(movement);
        }
        else
        {
            if (h != 0)
                MoveBoth(movement);

            _playerAnimator.SetFloat("XVelocity", h);
            //Caster.transform.position = transform.position;
        }

        _shadowAnimator.SetFloat("XVelocity", h);
    }

    private void MoveBoth(Vector2 movement)
    {
        transform.localScale = (movement.x < 0) ? new Vector3(-0.5f, 0.5f) : new Vector3(0.5f, 0.5f);

        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        _rigidbody.MovePosition(position + movement);
    }

    private void MoveShadow(Vector2 movement)
    {
        //If up and light zone move shadow ball
        if (_shadowBall.activeInHierarchy)
        {
            _shadowBall.transform.position += new Vector3(movement.x, movement.y);
        }
        else
        {
            //else move shadow
            Shadow.transform.localScale = (movement.x < 0) ? new Vector3(-1f, 1f) : new Vector3(1f, 1f);

            Shadow.transform.position += new Vector3(movement.x, movement.y);
        }

    }

    private void ActiveShadowBall()
    {
        _shadowBall.SetActive(true);
        IsActiveShadowBall = true;

        var startSize = _shadowBall.GetComponent<ParticleSystem>().startSize;
        var newStartSize = startSize + DISAPEARSPEED;
        _shadowBall.GetComponent<ParticleSystem>().startSize = Mathf.Clamp(newStartSize, 0, MAXSHADOWSIZE);

        var newBodyScale = Mathf.Clamp(1 - (newStartSize / MAXSHADOWSIZE), 0, 1);
        foreach (var item in Shadow.GetComponentsInChildren<Transform>())
        {
            item.localScale = new Vector3(newBodyScale, newBodyScale, newBodyScale);

        }

        if (newStartSize >= MAXSHADOWSIZE)
        {
            IsActiveShadowBall = false;
            Shadow.SetActive(false);
            CancelInvoke("ActiveShadowBall");
        }

    }

    private void UnActiveShadowBall()
    {
        Shadow.SetActive(true);
        IsUnActiveShadowBall = true;

        var startSize = _shadowBall.GetComponent<ParticleSystem>().startSize;
        var newStartSize = startSize - DISAPEARSPEED;
        _shadowBall.GetComponent<ParticleSystem>().startSize = Mathf.Clamp(newStartSize, 0, MAXSHADOWSIZE);

        var newBodyScale = Mathf.Clamp(1 - (newStartSize / MAXSHADOWSIZE), 0, 1);
        foreach (var item in Shadow.GetComponentsInChildren<Transform>())
        {
            item.localScale = new Vector3(newBodyScale, newBodyScale, newBodyScale);

        }

        if (newStartSize <= 0)
        {
            IsUnActiveShadowBall = false;
            _shadowBall.SetActive(false);
            CancelInvoke("UnActiveShadowBall");
        }

    }

    public void UnactiveBall()
    {
        if (!IsActiveShadowBall && !IsUnActiveShadowBall)
            InvokeRepeating("UnActiveShadowBall", 0, DISAPEARRATE);
    }
}
