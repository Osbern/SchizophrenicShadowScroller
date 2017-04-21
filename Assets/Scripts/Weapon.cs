using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Weapon : MonoBehaviour
{

    public GameObject bulletType;
    GameObject[] bullets;
    List<GameObject> toReactive;

    // Use this for initialization
    void Start()
    {
        bullets = GameObject.FindGameObjectsWithTag(StaticsTags.BULLETS);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > 0)
            {
                shoot();
            }
        }
    }

    void shoot()
    {

        foreach (GameObject bullet in bullets.ToList().OrderByDescending(x => x.name))
        {
            if (bullet.activeSelf)
            {
                if (toReactive == null)
                {
                    toReactive = new List<GameObject>();
                }
                bullet.SetActive(false);
                toReactive.Add(bullet);
                Instantiate(bullet, (Vector2)Input.mousePosition, Quaternion.identity);
                Invoke("reactiveRocketer", 5);
                break;
            }
        }

    }

    void reactiveRocketer()
    {
        if (toReactive.Count > 0)
        {
            toReactive[0].SetActive(true);
            toReactive.RemoveAt(0);
        }
    }
}
