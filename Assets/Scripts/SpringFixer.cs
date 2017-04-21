using UnityEngine;
using System.Collections;

public class SpringFixer : MonoBehaviour {

	// Use this for initialization
	void Start () {

        SpringJoint2D sj = GetComponent<SpringJoint2D>();
        if (sj!=null)
        {
            //GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);

            GetComponent<SpringJoint2D>().connectedAnchor =
                new Vector2(transform.position.x, transform.position.y+Random.Range(0,1));
        }
     
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
