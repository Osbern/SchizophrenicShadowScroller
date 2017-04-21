using UnityEngine;
using System.Collections;

public class TitleFallEffect : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Breaker")
        {
            gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        }

    }
}
