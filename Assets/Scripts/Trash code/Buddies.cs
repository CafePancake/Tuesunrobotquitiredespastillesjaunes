using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float _vitesse = 6f;
    private Rigidbody2D _rb;
    private Vector3 _dx;
    [SerializeField] private Obstacle _buddy;
    private bool _isHoldingHand = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _dx.y = -1;
    }
    void FixedUpdate()
    {
        _rb.MovePosition(transform.position + _dx * _vitesse * Time.fixedDeltaTime);
    }
    void OnBecameInvisible()
    {
        _isHoldingHand = true;
        Changerdx();
        _isHoldingHand = false;
    }

    void Changerdx()
    {
        
        _dx.y *=-1;
        if (_isHoldingHand)
        {
            _buddy.Changerdx();
            _vitesse=Random.Range(6f, 12f);
            _buddy._vitesse = _vitesse;
        }
        
    }
}
