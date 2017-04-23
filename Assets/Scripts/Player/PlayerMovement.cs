using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Component
    public GameObject Shadow;
    public GameObject ShadowBall;
    public float Speed;
    private bool _isShadowControlled;

    //Shadow
    public bool IsActiveShadowBall;
    public bool IsUnActiveShadowBall;

    //Player
    private Rigidbody2D _rigidbody;
    private Animator _playerAnimator;
    private Animator _shadowAnimator;

    //Timer
    private const float INPUT_DELAY = 0.5f;
    private const float DISAPEARSPEED = 0.02f;
    private const float DISAPEARRATE = 0.001f;
    private const float MAXSHADOWSIZE = 2f;
    private float _inputTimer;

    private bool _goToShadow = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();

        _shadowAnimator = Shadow.GetComponent<Animator>();

        ShadowBall.GetComponent<ParticleSystem>().startSize = 0;
        ShadowBall.SetActive(false);
    }

    private void FixedUpdate()
    {
        _inputTimer += Time.deltaTime;

        if (_goToShadow)
        {
            transform.position = Vector3.Lerp(transform.position, Shadow.transform.position, 0.3f);

            if (Vector3.Distance(transform.position, Shadow.transform.position) < 0.1f)
            {
                Shadow.transform.localPosition = new Vector3(-0.24f, -0.17f, 0);
                _goToShadow = false;
            }
        }
        else
        {
            if (Input.GetButton("Jump")
                     && _inputTimer >= INPUT_DELAY)
            {
                _isShadowControlled = !_isShadowControlled;

                if (!_isShadowControlled)
                {
                    _goToShadow = true;
                }

                _inputTimer = 0;
            }

            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            //ball or shadow
            var activeShadowBall = v > 0;
            if (_isShadowControlled
                && !ShadowBall.activeInHierarchy
                && activeShadowBall)
            {
                ShadowBall.transform.position = Shadow.transform.position;
                ActiveBall();
            }

            Vector2 movement = new Vector2(h, v);
            movement = movement.normalized * Speed * Time.deltaTime * (ShadowBall.activeInHierarchy ? 2 : 1);

            if (_isShadowControlled)
            {
                ShadowBall.SetActive(!Shadow.activeInHierarchy);
                //Shadow.GetComponent<Collider2D>().enabled = true;
                if (h != 0
                    || v != 0)
                    MoveShadow(movement);
            }
            else
            {
                //Shadow.GetComponent<Collider2D>().enabled = false;
                if (h != 0)
                    MoveBoth(movement);

                _playerAnimator.SetFloat("XVelocity", h);
                //Caster.transform.position = transform.position;
            }

            _shadowAnimator.SetFloat("XVelocity", h);
        }
    }

    private void MoveBoth(Vector2 movement)
    {
        //invert direction
        var scale = transform.localScale;
        if ((scale.x > 0 && movement.x < 0) || (scale.x < 0 && movement.x > 0))
        {
            scale.x *= -1;
            transform.localScale = scale;
        }
        scale = Shadow.transform.localScale;
        if ((scale.x > 0 && movement.x < 0) || (scale.x < 0 && movement.x > 0))
        {
            scale.x *= -1;
            Shadow.transform.localScale = scale;
        }

        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        _rigidbody.MovePosition(position + movement);

        Shadow.transform.position = transform.position + new Vector3(-0.1f, -0.02f, 0);
    }

    private void MoveShadow(Vector2 movement)
    {
        //If up and light zone move shadow ball
        if (ShadowBall.activeInHierarchy)
        {
            ShadowBall.transform.position += new Vector3(movement.x, movement.y);
        }
        else
        {
            //else move shadow
            //invert direction
            var scale = Shadow.transform.localScale;
            if ((scale.x > 0 && movement.x < 0) || (scale.x < 0 && movement.x > 0))
            {
                scale.x *= -1;
                Shadow.transform.localScale = scale;
            }

            Shadow.transform.position += new Vector3(movement.x, movement.y);
        }
    }

    private void ActiveShadowBall()
    {
        ShadowBall.SetActive(true);
        IsActiveShadowBall = true;

        var startSize = ShadowBall.GetComponent<ParticleSystem>().startSize;
        var newStartSize = startSize + DISAPEARSPEED;
        ShadowBall.GetComponent<ParticleSystem>().startSize = Mathf.Clamp(newStartSize, 0, MAXSHADOWSIZE);

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

        var startSize = ShadowBall.GetComponent<ParticleSystem>().startSize;
        var newStartSize = startSize - DISAPEARSPEED;
        ShadowBall.GetComponent<ParticleSystem>().startSize = Mathf.Clamp(newStartSize, 0, MAXSHADOWSIZE);

        var newBodyScale = Mathf.Clamp(1 - (newStartSize / MAXSHADOWSIZE), 0, 1);
        foreach (var item in Shadow.GetComponentsInChildren<Transform>())
        {
            item.localScale = new Vector3(newBodyScale, newBodyScale, newBodyScale);

        }
        if (newStartSize <= 0)
        {
            IsUnActiveShadowBall = false;
            ShadowBall.SetActive(false);
            CancelInvoke("UnActiveShadowBall");
        }
    }

    public void ActiveBall()
    {
        if (!IsActiveShadowBall
            && !IsUnActiveShadowBall)
        {
            InvokeRepeating("ActiveShadowBall", 0, DISAPEARRATE);
        }
    }

    public void UnactiveBall()
    {
        if (!IsActiveShadowBall && !IsUnActiveShadowBall)
            InvokeRepeating("UnActiveShadowBall", 0, DISAPEARRATE);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        ShadowBall.transform.position = Shadow.transform.position;
        ActiveBall();
    }
}
