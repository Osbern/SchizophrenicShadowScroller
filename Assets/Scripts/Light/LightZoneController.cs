using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightZoneController : MonoBehaviour
{
    public GameObject Player;
    public GameObject Shadow;

    //Mask
    private int _playerMask;

    //Collider
    private PolygonCollider2D _collider;
    private bool _isInLight;

    private void Awake()
    {
        _playerMask = LayerMask.NameToLayer("Mid1");
        _collider = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        Vector3 pos = Shadow.transform.position;
        pos.z = (_isInLight) ? Vector3.zero.z : -10f;
        Shadow.transform.position = pos;

        if (_isInLight)
        {
            float min = _collider.points.Min(v => v.x);
            float max = _collider.points.Max(v => v.x);

            float mid = ((max + min) / 2) + transform.position.x;
            float delta = Player.transform.position.x - mid;
            Shadow.transform.position = Player.transform.position + new Vector3(delta * 0.05f, 0f);
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _playerMask)
            _isInLight = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _playerMask)
            _isInLight = false;
    }
}
