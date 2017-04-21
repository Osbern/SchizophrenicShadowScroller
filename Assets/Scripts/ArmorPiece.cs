using UnityEngine;

public class ArmorPiece : MonoBehaviour
{

    public string bodyPart = "skull";
    public bool mirroring = false;
    public bool loot = false;

    // Use this for initialization
    void Start()
    {
        if (loot)
        {
            InitLoot();
        }
    }

    public void InitLoot()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            transform.localScale = GameObject.FindGameObjectWithTag("Player").transform.localScale;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

    }

    public void InitInventory()
    {
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void InitEquiped()
    {
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    void Update()
    {

    }
}
