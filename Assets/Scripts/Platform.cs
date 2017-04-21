using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class Platform : MonoBehaviour
{
    public GameObject platformPrevious, platformNext;
    List<GameObject> generated;
    float height, width;
    World world;

    void Start()
    {

    }

    public void Init(GameObject referencePlatform, bool next)
    {
        world = Camera.main.GetComponent<World>();

        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;

        if (referencePlatform != null)
        {
            if (next)
            {
                Vector3 offset = GetPlatformStart()
                           - referencePlatform.GetComponent<Platform>().GetPlatformEnd();

                //Decal between twice
                transform.position -= offset;

                platformPrevious = referencePlatform;
            }
            else
            {
                Vector3 offset = referencePlatform.GetComponent<Platform>().GetPlatformStart()
                         - GetPlatformEnd();

                //Decal between twice
                transform.position += offset;

                platformNext = referencePlatform;
            }
        }
        else
        {
            transform.position = new Vector2(0, -height / 2);
        }

        //IEnumerable<EdgeCollider2D> e = 
        //    GetComponentsInChildren<EdgeCollider2D>().Where(dd => dd.name == "GROUND");

        generated = new List<GameObject>();

        //If is not bridge 
        if (gameObject.GetComponentsInChildren<EdgeCollider2D>().Count() > 1)
        {
            int cpt = (int)(GetX()*width);
            foreach (var item in world.GetCurrentMap().getGenerated())
            {
                generated.Add(getRandomSizedObject(cpt++, item));
            }
        }

        if (referencePlatform == null)
        {
            platformPrevious = (GameObject)Instantiate(gameObject, Vector2.zero, Quaternion.identity);
            platformPrevious.GetComponent<Platform>().Init(gameObject, false);

            platformNext = (GameObject)Instantiate(gameObject, Vector2.zero, Quaternion.identity);
            platformNext.GetComponent<Platform>().Init(gameObject, true);
        }
    }

    public float GetYOnEdge(string startingEdge, string endingEdge = null)
    {
        float y0, y1, randomizedY;

        RaycastHit2D[] hits = Physics2D
            .RaycastAll(new Vector2(transform.position.x, 0), -Vector2.up);

        RaycastHit2D ray0 = hits.Where(x => x.collider.name == startingEdge).FirstOrDefault();
        y0 = ray0.point.y;

        if (string.IsNullOrEmpty(endingEdge))
        {
            return y0;
        }

        RaycastHit2D ray1 = hits.Where(x => x.collider.name == endingEdge).FirstOrDefault();
        y1 = ray1.point.y;

        randomizedY = Random.Range(y0, y1);

        return randomizedY;
    }

    public float GetX()
    {
        return transform.position.x;
    }

    public Vector3 GetPlatformStart()
    {
        EdgeCollider2D edegGround = gameObject.GetComponentsInChildren<EdgeCollider2D>()
             .Where(c => c.name == "GROUND").FirstOrDefault();

        return edegGround.points[0] + (Vector2)transform.position;
    }

    public Vector3 GetPlatformEnd()
    {
        EdgeCollider2D edegGround = gameObject.GetComponentsInChildren<EdgeCollider2D>()
            .Where(c => c.name == "GROUND").FirstOrDefault();

        return edegGround.points[edegGround.points.Length - 1] + (Vector2)transform.position;

    }

    GameObject getRandomSizedObject(int orderInLayer, GameObject what)
    {

        GameObject instanciated = null;
        GeneratedItem generated = what.GetComponent<GeneratedItem>();
        
        if (generated != null)
        {
            float randSize = what.transform.localScale.y;
            float randX, randY;

            if (generated.Resizable)
            {
                randSize = Random.Range(what.transform.localScale.y / 2, what.transform.localScale.y);
            }

            randX = Random.Range(GetX() - width / 2, GetX() + width / 2);

            randY = GetYOnEdge(generated.MinPopEdge, generated.MaxPopEdge);

            Vector2 randVector = new Vector2(randX, randY);
            instanciated = (GameObject)Instantiate(what, randVector, Quaternion.identity);

            GeneratedItem generatedInstanciated = instanciated.GetComponent<GeneratedItem>();

            instanciated.transform.parent = transform;
            instanciated.GetComponent<SpriteRenderer>()
                .sortingOrder = orderInLayer;

            Vector3 scale = new Vector3(
              (generatedInstanciated.Mirroreable ? (Random.Range(0, 5) > 2.5 ? 1 : -1) : 1) * randSize, randSize, 1
              );

            instanciated.transform.localScale = scale;
            
        }

        return instanciated;
    }
  
    GameObject getRandomPlacedSizedObject(GameObject what)
    {
 
        float randSize = Random.Range(what.transform.localScale.y / 2, what.transform.localScale.y);
        float offsetY = what.GetComponent<SpriteRenderer>().sprite.bounds.size.y
            - (what.GetComponent<SpriteRenderer>().sprite.bounds.size.y * randSize);

        Vector2 randVector = new Vector2(Random.Range(GetX() - width / 2, GetX() + width / 2), what.transform.position.y - offsetY / 2);
        GameObject g0 = (GameObject)Object.Instantiate(what, randVector, Quaternion.identity);


        g0.transform.localScale = new Vector2((Random.Range(0, 5) > 2.5 ? 1 : -1) * randSize, randSize);


        return g0;
    }

    private void destroyAll()
    {
        foreach (var item in generated)
        {
            Destroy(item, 0);
        }
        Destroy(gameObject, 0);
    }

    private void destroyNext()
    {
        if (platformNext != null)
        {
            platformNext.GetComponent<Platform>().destroyAll();
        }
    }

    private void destroyPrevious()
    {
        if (platformPrevious != null)
        {
            platformPrevious.GetComponent<Platform>().destroyAll();
        }
    }

    public void Enter(GameObject player)
    {
 
        if (platformPrevious == null && player.transform.position.x > transform.position.x)
        {
            platformPrevious = (GameObject)Instantiate(world.GetCurrentMap().getGround(), Vector2.zero, Quaternion.identity);
            platformPrevious.GetComponent<Platform>().Init(gameObject, false);
            platformNext.GetComponent<Platform>().destroyNext();

        }
        else if (platformNext == null)
        {
            platformNext = (GameObject)Instantiate(world.GetCurrentMap().getGround(), Vector2.zero, Quaternion.identity);
            platformNext.GetComponent<Platform>().Init(gameObject, true);
            platformPrevious.GetComponent<Platform>().destroyPrevious();
        }

    }

}
