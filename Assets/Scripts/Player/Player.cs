using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Movable
{
    private bool _isInLight;

    // Use this for initialization
    void Start()
    {
        Init();
        Enabled = true;
    }

    private float _distanceBetweenBoth = 0;
    private float disapearRate = 0.1f;
    private float disapearSpeed = 0.05f;
    // Update is called once per frame
    void Update()
    {
        if (GoToChild)
        {
            GetComponentInChildren<ParticleSystem>().Play();
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true;
            transform.position = Vector3.Lerp(transform.position, Child.transform.position, disapearSpeed);

            var distance = Vector3.Distance(transform.position, Child.transform.position);

            if (_distanceBetweenBoth == 0)
            {
                _distanceBetweenBoth = distance;
            }

            if (distance < 0.1f)
            {
                Child.transform.localPosition = new Vector3(-0.24f, -0.17f, 0);
                GoToChild = false;
                _distanceBetweenBoth = 0;
                transform.localScale = Vector3.one;
                GetComponent<Collider2D>().enabled = true;
                GetComponent<Rigidbody2D>().isKinematic = false;
                GetComponentInChildren<ParticleSystem>().Stop();
            }
            else
            {
                var firstPart = (distance < _distanceBetweenBoth / 2);

                foreach (var membre in GetComponentsInChildren<SpriteRenderer>())
                {
                    var color = membre.color;
                    var alpha = (distance / _distanceBetweenBoth) * 255;

                    var newAlpha = Mathf.Clamp(color.a + (firstPart ? disapearRate : -disapearRate), 0, 1);
                    membre.color = new Color(color.r, color.g, color.b, newAlpha);

                    var newScale = Child.transform.localScale * newAlpha;
                    transform.localScale = newScale;
                }
            }
        }
        else
        {
            if (Enabled)
            {
                Camera.main.GetComponent<FollowCam>().Target = this.gameObject;
                Child.GetComponent<Movable>().Enabled = false;
                Child.Child.GetComponent<Movable>().Enabled = false;

                _inputTimer += Time.deltaTime;

                if (Input.GetButton("Jump")
                    && _isInLight
                    && _inputTimer >= INPUT_DELAY)
                {
                    Child.GetComponent<Movable>().Activate(this);

                    _inputTimer = 0;
                }

                Move();
            }

        }
    }

    protected override Vector2 Move()
    {
        var movement = base.Move();

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

        //blocked
        //_animator.SetBool("Blocked", _blocked);
        //Child.GetComponent<Animator>().SetBool("Blocked", _blocked);

        Child.transform.position = transform.position + new Vector3(-0.1f, -0.02f, 0);

        return movement;
    }

    private bool _blocked = false;
    void OnCollisionEnter2D(Collision2D coll)
    {
        if ((LayerMask.LayerToName(coll.gameObject.layer) == "Collidable")
            && coll.transform.position.y < transform.position.y)
        {
            _blocked = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if ((LayerMask.LayerToName(coll.gameObject.layer) == "Collidable"))
        {
            _blocked = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "LightTrigger")
        {
            _isInLight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "LightTrigger")
        {
            _isInLight = false;
        }
    }
}
