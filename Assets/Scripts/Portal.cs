using UnityEngine;

public class Portal : MonoBehaviour
{
    void Start()
    {
        float width = 2f * Camera.main.orthographicSize * Camera.main.aspect;
        float x = Camera.main.transform.position.x + width - gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        transform.position = new Vector2(x, transform.position.y);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.tag == StaticsTags.PLAYER)
        {

            Camera.main.GetComponent<World>().NextMap();

            GameObject[] ennemies = GameObject.FindGameObjectsWithTag(StaticsTags.ENNEMY);

            foreach (var ennemy in ennemies)
            {
                Destroy(ennemy, 0);
            }

        }
    }
}
