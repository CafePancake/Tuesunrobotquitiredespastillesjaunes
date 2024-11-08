using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaProjectiles : MonoBehaviour
{
    private float _vitesse = 6f; //vitesse du projectile 
    private Vector3 _target; //position vis/e
    private float _targetX; //position du target en x
    private float _targetY; //position du target en y
    private float _posX; //position en x
    private float _posY; //position en y
    private float _rotation; //rotation du projectile

    void Start()
    {
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition); //target est la ou la souris pointe
        _rotation = transform.rotation.z;
        _posX = transform.position.x;
        _posY = transform.position.y;
        _targetX = _target.x;
        _targetY = _target.y;
        _rotation = Mathf.Atan2(_posX-_targetX, _posY-_targetY)*Mathf.Rad2Deg;
        transform.Rotate(0f,0f,-_rotation); //rotatione la projectile selon la tan entre projectile et cible
    }

    void Update()
    {
        transform.Translate(_vitesse * Time.deltaTime * Vector3.down, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player"))
        {
            Destroy(gameObject); //le projectile est detruit si il entre en contact avec autre que joueur
        }       
    }
    
}

