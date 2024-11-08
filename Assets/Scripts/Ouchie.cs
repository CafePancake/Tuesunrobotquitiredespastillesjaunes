using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ouchie : MonoBehaviour
{
    private float _vitesse = 5f; //vitesse du projectile
    [SerializeField] private GameObject _target; //object qui sera suivi par le projectile
    private Vector3 _coordinates; //position desiree du projectile
    private Rigidbody2D _rb; //rigidbody du gameobject
    private float _lifetime = 3f; //duree de vie du projectile

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _target = GameObject.Find("Player");
        Invoke("Explode", _lifetime); //explose dans 3 secondes
    }

    void Update()
    {
        _vitesse+=0.005f*Time.deltaTime; //projectile acclere legerement au fil du temps
        _coordinates = _target.transform.position; //position desiree est la position du target
        transform.position = Vector2.MoveTowards(transform.position, _coordinates, _vitesse * Time.deltaTime);
    }

    void Explode()
    {
        Destroy(gameObject);
    }
}
