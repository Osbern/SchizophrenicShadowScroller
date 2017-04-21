using UnityEngine;
using System.Collections;

public class Lootable : MonoBehaviour
{
    public int Amount = 1;
    public bool Coin = true;
    bool readytoloot = false;
    private  AudioSource myAudio;
    public bool Eatable = true;
    public bool Pop;
    // Use this for initialization
    void Start()
    {
        myAudio = gameObject.GetComponent<AudioSource>();
        if (Pop)
        {
            Rigidbody2D r2d = GetComponent<Rigidbody2D>();
            int randX = Random.Range(-5, 5);

            r2d.AddForce(Vector2.up * 300);
            r2d.AddForce(Vector2.right * randX * 10);
            r2d.AddTorque(randX);
        }
        Invoke("ReadyToLoot", 1);
    }

    void ReadyToLoot()
    {
        readytoloot = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        loot(collision.collider);
       
       
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        loot(collider);


    }

    void OnCollisionStay2D(Collision2D collision)
    {
        loot(collision.collider);
    }

    void loot(Collider2D collider)
    {
  
        if (readytoloot)
        {
            if (collider.tag == StaticsTags.PLAYER)
            {
                Stats.AddStatCounter(Stats.COUNT_SCORE ,Amount);

                if (myAudio != null)
                {
                    AudioSource.PlayClipAtPoint(myAudio.clip, transform.position);
                }

                Destroy(gameObject, 0.01f);
            }
            if (Eatable && collider.tag == StaticsTags.ENNEMY || collider.tag == StaticsTags.FLYERS)
            {
                collider.gameObject.GetComponent<IA>().Eat(gameObject);
                gameObject.SetActive(false);
            }

            if (transform.rotation != Quaternion.identity)
            {
                transform.rotation = Quaternion.identity;
            }
        }
    }
}
