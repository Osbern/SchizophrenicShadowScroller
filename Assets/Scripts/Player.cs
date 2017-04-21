using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float Force = 50;
    public float Speed = 3.0f;
    public float JumpForce = 4.0f;
    public bool Active = true;
    private AudioSource myAudio;

    public GameObject Hat, Coat
                    , LeftShoulder, LeftWrist, LeftHand, LeftLeg, LeftShinBone, LeftFoot
                    , RightShoulder, RightWrist, RightHand, RightLeg, RightShinBone, RightFoot;

    static int startingFragsLeft = 10;
    private int fragsLeft = startingFragsLeft;

    private List<string> bodyParts = new List<string>()
        { "skull", "torso",
        "leftArm" , "leftWrist", "leftHand" , "leftLeg" , "leftShinbone","leftFoot",
        "rightArm","rightWrist", "rightHand", "rightLeg","rightShinbone", "rightFoot"
        };

    private Dictionary<string, GameObject> playerBodyParts, playerEquipedWear;

    Rigidbody2D r2d;
    Animator animator;
    Vector2 lastPosition;
    Vector3 mouseToScreen;
    public static int IDLE = 0, WALK = 1, JUMP = 2;
    private float lastClick;
    private bool doubleClic = false;
    private int jumpCount = 0;

    // Use this for initialization
    void Start()
    {
        myAudio = gameObject.GetComponent<AudioSource>();
        r2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        setBodyParts();
        wear();
        resetFrags();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.timeScale > 0.0f)
        {
            doubleClic = false;

            if (Input.GetMouseButtonDown(0))
            {
                if (lastClick > 0)
                {
                    doubleClic = (Time.time - lastClick) < 0.3f;
                }
                lastClick = Time.time;
            }

            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Stats.AddStatCounter(Stats.COUNT_CLICK);
                }

                mouseToScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (mouseToScreen.y <= 0)
                {
                    if (Active)
                    {

                        if (doubleClic && jumpCount < 2)
                        {
                            jump();
                        }

                        float x = mouseToScreen.x - transform.position.x;

                        if (Mathf.Abs(x) > 1.0f)
                        {
                            Vector3 theScale = transform.localScale;
                            if (theScale.x < 0 && x > 0 || theScale.x > 0 && x < 0)
                            {
                                theScale.x *= -1;
                                transform.localScale = theScale;
                            }

                            lastPosition = mouseToScreen;
                            r2d.velocity = new Vector2(
                                (Vector2.right
                                * Speed //* Mathf.Abs(x)/4
                                * (lastPosition.x - transform.position.x < 0 ? -1 : 1)).x, r2d.velocity.y);

                            Stats.AddStatCounter(Stats.COUNT_DISTANCE, Mathf.Max(transform.position.x, lastPosition.x) - Mathf.Min(transform.position.x, lastPosition.x));

                        }
                    }
                }
            }
            else
            {
                r2d.velocity = new Vector2(0, r2d.velocity.y);
            }
            animator.SetFloat("XVelocity", r2d.velocity.x);
            animator.SetFloat("YVelocity", r2d.velocity.y);
        }

    }

    void setBodyParts()
    {
        if (playerBodyParts == null)
        {
            playerBodyParts = new Dictionary<string, GameObject>();
            playerEquipedWear = new Dictionary<string, GameObject>();
        }
        foreach (Transform item in gameObject.GetComponentsInChildren<Transform>())
        {
            if (bodyParts.Contains(item.name))
            {
                playerBodyParts.Add(item.name, item.gameObject);
                playerEquipedWear.Add(item.name, null);
            }
        }
    }

    void RayTest()
    {

        if (r2d.velocity.y == 0)
        {
            //Vector2 v2 = new Vector2(transform.position.x + (onLeft ? 1 : -1), transform.position.y - 0.5f);

            //RaycastHit2D hit = Physics2D.Raycast(v2, onLeft ? Vector2.right : -Vector2.right);
            //if (hit.collider != null && hit.distance < 0.5f && hit.collider.tag == StaticsTags.ENNEMY)
            //{
            //    jump();
            //}

        }
    }

    void jump()
    {
        jumpCount++;
        if (jumpCount > 1)
        {
            animator.Play("DoubleJump");
            Stats.AddStatCounter(Stats.COUNT_DOUBLEJUMP);
        }
        else
        {
            Stats.AddStatCounter(Stats.COUNT_JUMP);
        }

        animator.SetBool("Jumping", true);
        r2d.velocity = new Vector2(r2d.velocity.x, (Vector2.up).y * JumpForce * (jumpCount > 1 ? 1.2f : 1));
        if (myAudio != null)
        {
            AudioSource.PlayClipAtPoint(myAudio.clip, transform.position);
        }
    }

    void hurt(Collider2D hurted)
    {
        if (getYMid(gameObject) > getYMid(hurted.gameObject))
        {
         
            jumpCount = 0;
            jump();
            if (hurted.GetComponent<IA>().Hit(Force * (jumpCount > 1 ? 1.5f : 1)))
            {
                fragsLeft--;
                if (hurted.GetComponent<IA>().IsBoss)
                {
                    resetFrags();
                    Stats.AddStatCounter(Stats.COUNT_BOSS);
                }
                else
                {
                    Stats.AddStatCounter(Stats.COUNT_ENNEMIES);
                }
            }
        }

    }

    private void resetFrags()
    {
        fragsLeft = startingFragsLeft + Mathf.Clamp(Camera.main.GetComponent<World>().GetLevel(), 0, 20);
    }

    public bool FragsOk()
    {
        return fragsLeft <= 0;
    }

    float getYMid(GameObject go)
    {

        if (go.GetComponent<CircleCollider2D>() != null)
        {
            return go.transform.position.y + go.GetComponent<CircleCollider2D>().offset.y;
        }
        else if (go.GetComponent<BoxCollider2D>() != null)
        {
            return go.transform.position.y + go.GetComponent<BoxCollider2D>().offset.y;
        }
        return 0;
    }

    float getYTop(GameObject go)
    {

        if (go.GetComponent<CircleCollider2D>() != null)
        {
            return go.transform.position.y + go.GetComponent<CircleCollider2D>().offset.y
               + go.GetComponent<CircleCollider2D>().radius;
        }
        else if (go.GetComponent<BoxCollider2D>() != null)
        {
            return go.transform.position.y + go.GetComponent<BoxCollider2D>().offset.y
                + go.GetComponent<BoxCollider2D>().bounds.size.y / 2;
        }
        return 0;
    }

    float getYBot(GameObject go)
    {
        if (go.GetComponent<CircleCollider2D>() != null)
        {
            return go.transform.position.y + go.GetComponent<CircleCollider2D>().offset.y
                - go.GetComponent<CircleCollider2D>().radius;
        }
        else if (go.GetComponent<BoxCollider2D>() != null)
        {
            return go.transform.position.y + go.GetComponent<BoxCollider2D>().offset.y
                - go.GetComponent<BoxCollider2D>().bounds.size.y / 2;
        }
        return 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == "GROUND"
            || collision.collider.gameObject.name == "PLATFORM")
        {
            animator.SetBool("Jumping", false);
            if (collision.collider.gameObject.name == "GROUND")
            {
                collision.collider.transform.parent
                .GetComponent<Platform>().Enter(gameObject);
            }
        
            jumpCount = 0;
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.tag == StaticsTags.ENNEMY)
        {
            hurt(collider);
        }

    }

    void setWearPart(GameObject wearModel, string bodyPartToEquip)
    {
        if (wearModel != null)
        {
            GameObject bodyPart;
            playerBodyParts.TryGetValue(bodyPartToEquip, out bodyPart);
            GameObject wearPart;
            playerEquipedWear.TryGetValue(bodyPartToEquip, out wearPart);

            if (wearPart != null)
            {
                Destroy(wearPart);
            }

            wearPart = (GameObject)Instantiate(wearModel, bodyPart.transform.position, Quaternion.identity);
            if (wearPart.GetComponent<ArmorPiece>() != null)
            {
                wearPart.GetComponent<ArmorPiece>().InitEquiped();
            }

            wearPart.transform.parent = bodyPart.transform;
            wearPart.transform.localScale = bodyPart.transform.localScale;

            playerEquipedWear[bodyPartToEquip] = wearPart;

        }

    }

    void wear()
    {
        setWearPart(Hat, "skull");
        setWearPart(Coat, "torso");
        setWearPart(RightShoulder, "rightArm");
        setWearPart(RightWrist, "rightWrist");
        setWearPart(RightHand, "rightHand");
        setWearPart(RightLeg, "rightLeg");
        setWearPart(RightShinBone, "rightShinbone");
        setWearPart(RightFoot, "rightFoot");
        setWearPart(LeftShoulder, "leftArm");
        setWearPart(LeftWrist, "leftWrist");
        setWearPart(LeftHand, "leftHand");
        setWearPart(LeftLeg, "leftLeg");
        setWearPart(LeftShinBone, "leftShinbone");
        setWearPart(LeftFoot, "leftFoot");

    }

}
