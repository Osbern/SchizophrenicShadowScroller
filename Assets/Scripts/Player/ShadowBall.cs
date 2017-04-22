using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBall : MonoBehaviour
{
    public PlayerMovement PlayerMovement;
    public GameObject Shadow;

    private Vector3 _offset;
    // Use this for initialization
    void Start()
    {
        _offset = transform.position - Shadow.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Active(bool active)
    {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //var shadowBallTouchGround = (LayerMask.LayerToName(coll.gameObject.layer) == "Floor");

        //if (shadowBallTouchGround)
        //{
        PlayerMovement.UnactiveBall();
        Shadow.SetActive(true);
        Shadow.transform.position = transform.position + Vector3.up * 0.45f;

        //}

    }
}
