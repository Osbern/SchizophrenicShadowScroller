using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour
{

    public GameObject Owner;
    Vector2 offset;
    GameObject target;
    float speed;
    public bool onLeft = false;

    // Use this for initialization
    void Start()
    {
        if (Owner != null)
        {
            offset = transform.position - Owner.transform.position;
            target = Owner;
            speed = 0.02f;
        }
        else
        {
            searchTarget();
            speed = 0.05f;
        }

    }

    void searchTarget()
    {
        if (target == null)
        {
            GameObject[] flyers = GameObject.FindGameObjectsWithTag(StaticsTags.FLYERS);
            float nearest = 1000;
            foreach (GameObject flyer in flyers)
            {
                if (flyer.layer == LayerMask.NameToLayer("Friends"))
                {
                    float dist = Vector2.Distance(transform.position, flyer.transform.position);
                    if (dist < nearest)
                    {
                        nearest = dist;
                        target = flyer;
                    }
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            searchTarget();
        }
        if (target != null)
        {
            if (Owner == null)
            {
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)target.transform.position, speed);
            }
            else
            {
                transform.position = Vector2.Lerp(transform.position, (Vector2)target.transform.position + offset, speed);
            }

            float x = target.transform.position.x - transform.position.x;
            if ((x < 0 && onLeft) || (x > 0 && !onLeft))
            {
                onLeft = x > 0;
                changeDirection();
            }
        }


    }

    void changeDirection()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
