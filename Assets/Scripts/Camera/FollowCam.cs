using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    private Vector3 _offset;
    public GameObject Target;
    public float Smooth = 0.5f;

    // Use this for initialization
    void Start()
    {
        _offset = transform.position - Target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
