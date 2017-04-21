using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{

    public float spawnTime = 0f;
    public float spawnDistance = 0f;
    public GameObject Portals;
    private GameObject portal;
    private Vector2 lastSpawnPos;
    float height, width;

    // Use this for initialization
    void Start()
    {
        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
    }

    public void StartSpawning()
    {
        if (portal != null)
        {
            Destroy(portal);
        }
        if (spawnDistance > 0)
        {
            InvokeRepeating("Spawn", 1, 1);
        }
        if (spawnTime > 0)
        {
            InvokeRepeating("Spawn", spawnTime, spawnTime);
        }

    }

    void Spawn()
    {
        if (GameObject.FindGameObjectWithTag(StaticsTags.PLAYER)!=null)
        {
            if (GameObject.FindGameObjectWithTag(StaticsTags.PLAYER).GetComponent<Player>().FragsOk())
            {
                CancelInvoke("Spawn");

                portal = (GameObject)Instantiate(Portals, new Vector2(Camera.main.transform.position.x, Portals.transform.position.y), Quaternion.identity);

                foreach (GameObject particle in GameObject.FindGameObjectsWithTag(StaticsTags.PORTAL))
                {
                    if (particle.GetComponent<ParticleSystem>() != null)
                    {
                        particle.GetComponent<ParticleSystem>().startColor = Camera.main.GetComponent<World>().GetCurrentMap().GetPortalsColors();
                    }
                }

                Instantiate(Camera.main.GetComponent<World>().GetCurrentMap().getBoss(), transform.position, Quaternion.identity);
            }
            else
            {
                if ((spawnTime > 0)
                    || (spawnDistance > 0
                        && Mathf.Abs(lastSpawnPos.x - gameObject.transform.position.x) > spawnDistance * width))
                {
                    lastSpawnPos = transform.position;
                    Instantiate(Camera.main.GetComponent<World>().GetCurrentMap().getEnnemy(), transform.position, Quaternion.identity);
                }

            }
        }
       

    }

}
