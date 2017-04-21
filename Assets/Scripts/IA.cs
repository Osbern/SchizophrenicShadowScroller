using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour
{

    public float SpeedMin = 1.0f;
    public float SpeedMax = 1.0f;

    public bool RandScale = false;
    public bool RandSpawn = false;
    public bool InverseSprite = false;
    public Color[] Colors;
    public float DetectionArea = 0.3f;

    public string OnAttackAnimation = "Attack";
    public string OnIdleAnimation = "Idle";
    public string OnWalkAnimation = "Walk";
    public string OnRunAnimation = "Run";
    public string OnLowerAnimation = "Lower";
    public string OnRaiseAnimation = "Raise";
    public string OnKillAnimation = "";

    public float Life = 100;
    public GameObject[] Loots;
    public AudioClip audioDye;
    public bool IsBoss;

    List<GameObject> eatedGameObjects;

    private float attackRange = 0.1f;
    GameObject player;
    Vector2 direction = Vector2.right;
    float width, height;
    float spriteWidth;
    float speed;
    bool raised = false;
    private Animator animator;
    private string stance;

    // Use this for initialization
    void Start()
    {
        eatedGameObjects = new List<GameObject>();

        stance = OnIdleAnimation;

        animator = gameObject.GetComponent<Animator>();

        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
        spriteWidth = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;

        if (RandSpawn)
        {
            setRandomSpawn();
        }
        speed = Random.Range(SpeedMin, SpeedMax);

        if (Colors.Length > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Colors[Random.Range(0, Colors.Length)];
        }

        if (RandScale)
        {
            float randScale = Random.Range(0.5f, 1.0f);
            transform.localScale = new Vector2(randScale, randScale);
        }

        if (transform.position.x > Camera.main.transform.position.x)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= InverseSprite ? -1 : 1;
            direction = Vector2.left;
            transform.localScale = theScale;
        }

    }

    void changeDirection()
    {
        direction = -direction;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void setRandomSpawn()
    {
        bool left = Random.Range(0, 10) > 5;

        transform.position = new Vector2(
             left == true ? Camera.main.transform.position.x - width / 2 - spriteWidth : Camera.main.transform.position.x + width / 2 + spriteWidth,
            Random.Range(Camera.main.transform.position.y, Camera.main.transform.position.y + height / 2));

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag(StaticsTags.PLAYER);
        }

        if (gameObject.tag == StaticsTags.ENNEMY && Life > 0 && player != null)
        {
            float distance = Mathf.Abs(player.transform.position.x - transform.position.x);

            if (SpeedMin == 0)
            {
                if (!raised && distance < DetectionArea * width)
                {
                    raised = true;
                    animator.Play(OnRaiseAnimation);
                }
                else
                {
                    if (raised && distance >= DetectionArea * width)
                    {
                        raised = false;
                        animator.Play(OnLowerAnimation);
                    }
                }
            }
            else
            {
                if ((direction.x > 0 && player.transform.position.x > transform.position.x)
                    || (direction.x < 0 && player.transform.position.x < transform.position.x))
                {
                    if (IsBoss || distance < DetectionArea * width)
                    {
                        transform.Translate(direction * speed * Time.deltaTime);

                        if (distance < attackRange * width)
                        {
                            setStance(OnAttackAnimation);
                        }
                        else
                        {
                            if (speed > 2)
                            {
                                setStance(OnRunAnimation);
                            }
                            else
                            {
                                setStance(OnWalkAnimation);
                            }
                        }
                    }
                    else
                    {
                        setStance(OnIdleAnimation);
                    }
                }
                else
                {
                    transform.Translate(direction * speed * Time.deltaTime);
                    if (speed > 2)
                    {
                        setStance(OnRunAnimation);
                    }
                    else
                    {
                        setStance(OnWalkAnimation);
                    }
                }
            }

        }
        else
        {
            if (transform.position.x < Camera.main.transform.position.x - width / 2 - spriteWidth * 5
                || transform.position.x > Camera.main.transform.position.x + width / 2 + spriteWidth * 5)
            {
                if (RandSpawn)
                {
                    Instantiate(gameObject);
                }
                Destroy(gameObject, 0);
            }
        }

    }

    void setStance(string newStance)
    {
        if (stance != newStance)
        {
            stance = newStance;
            animator.Play(stance);

        }
    }

    public bool Hit(float force)
    {

        Life -= force;

        if (Life <= 0)
        {
            if (Loots.Length > 0)
            {
                if (IsBoss)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Instantiate(Loots[Random.Range(0, Loots.Length)], transform.position, Quaternion.identity);
                    }
                    Camera.main.GetComponent<World>().NextMap();
                }
                Instantiate(Loots[Random.Range(0, Loots.Length)], transform.position, Quaternion.identity);
            }

            foreach (GameObject loot in eatedGameObjects)
            {
                if (loot != null)
                {
                    loot.SetActive(true);
                }

            }

            if (audioDye != null)
            {
                AudioSource.PlayClipAtPoint(audioDye, transform.position);
            }
            if (OnKillAnimation != "")
            {
                animator.Play(OnKillAnimation);

                gameObject.GetComponent<BoxCollider2D>().enabled = false;

                if (gameObject.GetComponent<Rigidbody2D>() != null)
                {
                    gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                }

                InvokeRepeating("flash", 0, 0.2f);
                Destroy(gameObject, 1.5f);
            }
            else
            {
                Destroy(gameObject, 0.1f);
            }


            return true;
        }
        return false;
    }


    void flash()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;

    }

    public void Eat(GameObject what)
    {
        eatedGameObjects.Add(what);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == StaticsTags.PORTAL)
        {
            changeDirection();
        }

    }

}
