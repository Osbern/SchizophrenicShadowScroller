using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour
{
    public GameObject Destination;
    public AudioClip LootClip;

    // Use this for initialization
    void Start()
    {

    }

    public void LootAudio()
    {
        AudioSource.PlayClipAtPoint(LootClip, transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if ((LayerMask.LayerToName(coll.gameObject.layer) == "Body"))
        {
            if (Destination != null)
            {
                GetComponent<Collider2D>().enabled = false;
                transform.parent = Destination.transform;
                transform.localPosition = Vector3.zero - Vector3.up * 0.6f;
            }
            else
            {
                GetComponent<SpriteRenderer>().enabled = false;
                transform.parent = coll.gameObject.transform;
            }
            Destroy(GetComponent<Rigidbody2D>());

            GetComponent<Collider2D>().enabled = false;
            LootAudio();
        }


    }
}
