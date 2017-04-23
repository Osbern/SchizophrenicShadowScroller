using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Movable
{

    // Use this for initialization
    void Start()
    {
        Init();
        Enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GoToChild)
        {
            transform.position = Vector3.Lerp(transform.position, Child.transform.position, 0.3f);

            if (Vector3.Distance(transform.position, Child.transform.position) < 0.1f)
            {
                Child.transform.localPosition = new Vector3(-0.24f, -0.17f, 0);
                GoToChild = false;
            }
        }
        else
        {
            if (Enabled)
            {
                _inputTimer += Time.deltaTime;

                if (Input.GetButton("Jump")
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
}
