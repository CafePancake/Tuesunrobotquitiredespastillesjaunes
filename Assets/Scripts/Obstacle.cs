using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private float _vitesse = 3f; //vitesse de l<obstacle
    private Rigidbody2D _rb; //rigidbody du gameobject
    private Vector3 _dx; //direction du mouvement (haut/bas)
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _dx.y = -1;
    }
    void FixedUpdate()
    {
        _rb.MovePosition(transform.position + _dx * _vitesse * Time.fixedDeltaTime);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mur"))
        {
            Changerdx(); //change de direction quand entre en contact avec un mur
        }
    }

    void Changerdx()
    {
        _dx.y *=-1; //inverse la direction
        _vitesse=Random.Range(3f, 6f); //vitesse aleatoire
    }
}
