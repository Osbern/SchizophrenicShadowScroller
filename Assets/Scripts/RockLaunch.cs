using UnityEngine;
using System.Collections;

public class RockLaunch : MonoBehaviour
{

    Vector3 rot;
    // Use this for initialization
    void Start()
    {
        rot = new Vector3(0, 0, Random.Range(0, 10) < 5 ? -1 : 1);

    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(rot * Time.deltaTime * 500);
        transform.localScale += new Vector3(-0.1f, -0.1f, 0);

        if (transform.localScale.x <= 0.1f || transform.localScale.y <= 0.1f)
        {
            GameObject[] flyers = GameObject.FindGameObjectsWithTag(StaticsTags.FLYERS);
            foreach (GameObject flyer in flyers)
            {
                if (flyer.layer == LayerMask.NameToLayer("Ennemies"))
                {
                    if (flyer.GetComponent<BoxCollider2D>().bounds.Contains(transform.position))
                    {
                        flyer.GetComponent<IA>().Hit(100);

                    }
                }
            }
            Destroy(gameObject, 0);
        }

    }
}
