using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour
{
    public GameObject Destination;

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if ((LayerMask.LayerToName(coll.gameObject.layer) == "Body"))
        {
            if (Destination != null)
            {
                GetComponent<Collider2D>().enabled = false;
                transform.parent = Destination.transform;
                transform.localPosition = Vector3.zero - Vector3.up * 0.6f;
                Vector3 scale = transform.localScale;
                scale.x = (Destination.transform.lossyScale.x > 0) 
                                                        ? transform.localScale.x
                                                        : -transform.localScale.x;
                transform.localScale = scale;
                
            }
            else
            {
                GetComponent<SpriteRenderer>().enabled = false;
                transform.parent = coll.gameObject.transform;
            }
            Destroy(GetComponent<Rigidbody2D>());

            GetComponent<Collider2D>().enabled = false;
        }


    }
}
