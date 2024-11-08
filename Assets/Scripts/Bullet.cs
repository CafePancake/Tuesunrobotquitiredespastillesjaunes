using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CheeseHolers : MonoBehaviour
{
    private float _vitesse = 8f; //vitessse du projectile
    private float _rotation; //rotation angle du projectile
    private GameObject _player; //gameobject du joueur
    private float _posX; //position en x
    private float _posY; //position en y
    private float _playerPosX; //position du joueur en x
    private float _playerPosY; // position du joueur n y 

    void Start()
    {
        _rotation = transform.rotation.z; //???
        _player = GameObject.Find("Player");
        _posX = transform.position.x;
        _posY = transform.position.y;
        _playerPosX = _player.transform.position.x;
        _playerPosY = _player.transform.position.y;
        _rotation = Mathf.Atan2(_posX-_playerPosX, _posY-_playerPosY)*Mathf.Rad2Deg;
        transform.Rotate(0f,0f,-_rotation); //calcule la rotation du projectile selon la tan entre projectile et joueur
    }

 
    void Update()
    { 
        transform.Translate(_vitesse * Time.deltaTime * Vector3.right, Space.Self);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Boss")&&!other.CompareTag("Missile"))
        {
            Destroy(gameObject);
        }
    }
}
