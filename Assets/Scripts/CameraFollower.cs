using UnityEngine;

public class CameraFollower : MonoBehaviour
{

    Vector3 offset;
    GameObject target;
    float horizontalSpeed = 0.015f, verticalSpeed = 1.0f, initedZ;
    Vector2 horizontalBuffer, verticalBuffer;


    // Use this for initialization
    void Start()
    {
    }

    void setTarget()
    {
        
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag(StaticsTags.PLAYER);
            if (target!=null)
            {
                offset = transform.position - target.transform.position;
                initedZ = transform.position.z;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        setTarget();
        if (target != null)
        {
            horizontalBuffer = Vector3.Lerp(transform.position, target.transform.position + offset, horizontalSpeed);
            verticalBuffer = Vector3.Lerp(transform.position, target.transform.position + offset, verticalSpeed);
            transform.position = new Vector3(horizontalBuffer.x, verticalBuffer.y, initedZ);

        }

    }

}
