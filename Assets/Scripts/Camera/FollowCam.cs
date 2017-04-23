using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    private Vector3 _offset;
    public GameObject Target;
    public float Smooth = 0.5f;
    private bool _canFly;

    // Use this for initialization
    void Start()
    {
        _offset = transform.position - Target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = Target.transform.position + _offset;

        if (!_canFly)
            newPos.y = 0f;

        transform.position = Vector3.Lerp(transform.position, newPos, Smooth);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        _canFly = (collision.GetComponent<Collider>().tag == "YCamTrigger");
    }
}
