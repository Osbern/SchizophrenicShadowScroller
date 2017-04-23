using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBall : Movable
{
    public float Size = 0.5f;

    void Start()
    {
        Init();

        GetComponent<ParticleSystem>().startSize = Size;
        gameObject.GetComponent<ParticleSystem>().Stop();
    }


    // Update is called once per frame
    void Update()
    {
        if (Enabled)
        {

            Move();
        }
    }

    protected override Vector2 Move()
    {
        var movement = base.Move();

        //move
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        _rigidbody.MovePosition(position + movement);

        return movement;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //var shadowBallTouchGround = (LayerMask.LayerToName(coll.gameObject.layer) == "Floor");

        //if (shadowBallTouchGround)
        //{
        //GetComponentInParent<PlayerMovement>().UnactiveBall();
        //Shadow.SetActive(true);
        //Shadow.transform.position = transform.position + Vector3.up * 0.45f;
        //if (shadowBallTouchGround)
        //{

        if (Enabled)
        {
            if (coll.transform.position.y < transform.position.y)
            {
                gameObject.GetComponent<ParticleSystem>().Stop();
                Parent.transform.position = transform.position + Vector3.up * DudeSize;
                Parent.GetComponent<Movable>().Activate(this);
                Parent.GetComponent<Shadow>().Apear();
            }

        }


        //}

        //}
    }
}
