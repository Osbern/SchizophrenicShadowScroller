using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTuto : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
