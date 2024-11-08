using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


public class Chara : MonoBehaviour
{
    private float _vitesseBase = 6f; //vitesse de base du joueur
    private float _vitesse; //vitesse actualle du joueur
    private float _vitesseBoost = 12f; //vitesse de sprint du joueur
    private int _energy = 90; //energie de la batterie du joueur (%)
    private int _energyWallPower = 5; //le nombre de % d<energie ajoute par le contact d<un obstacle par tick
    private int _energyProjectilePower = 10; //energie ajoute contact projectile ennemi
    private float _tickTime = 0.1f; //temps dun tick (ici 0.1 sec)
    private int _energyLossPerSec = 2; //energie perdue par seconde
    private int _projectileCost = 10; //cout en energie d<un projectile
    const int _VIES_MAX = 3; //nbvies maximum du joueur
    private int _vies; //nbvies actuelles du joueur
    private bool _isOverloaded; //bool si %energie est trop haut ou bas
    private Animator _anim; //animator qui gere les animations du perso
    private Rigidbody2D _rigidB; //rigidbody du gameobject
    private Vector3 _dx; //direction des inputs de mouvement
    [SerializeField] private TMP_Text _texteCharge; //texte qui affiche l<energie de la batterie
    [SerializeField] private SoundManager _soundManager; //manager qui gere les sons du jeu
    [SerializeField] private AudioClip _sontir; // son quand le joeuur tire
    [SerializeField] private AudioClip _sonEnergyWall; //son quand le joueur passe dans un obstacle
    [SerializeField] private AudioClip _sonViePerdue; //son quan dle joueur perd une vie
    [SerializeField] private GameObject[] _heartContainers; //srites qui repr/sentent les vies du perso
    [SerializeField] private GameManager _manager; //manager qui s<occupe de fonctions qui ont relations avec divers objects
    [SerializeField] private GameObject _projectile; //prefab projectile du joueur
    [SerializeField] private Transform[] _SpawnPoints; //positions de 3 spawns dans la map
    void Start()
    {
        _anim = GetComponent<Animator>();
        _vies = _VIES_MAX;
        StartCoroutine("LoseEnergyPerSec");
        _vitesse = _vitesseBase;
        _rigidB = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdateCharge",0f,0.1f);
    }
    void FixedUpdate()
    {   
        _dx.x = Input.GetAxisRaw("Horizontal");
        _dx.y = Input.GetAxisRaw("Vertical");
        _dx.Normalize();
        _rigidB.MovePosition(transform.position+_vitesse*Time.fixedDeltaTime*_dx); //d/placement du perso
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) //press shift to run
        {
            _vitesse = _vitesseBoost;
            _energyLossPerSec = 6; //augmente le drain d<energie quand le perso cours
        }
        else
        {
            _vitesse = _vitesseBase;
            _energyLossPerSec = 2;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0)) //souris pour tirer
        {
            if(_energy>0)
            {
                Instantiate(_projectile, transform.position, Quaternion.identity);
                _energy-=_projectileCost; //tirer coute de l<energie
                _soundManager.JouerSon(_sontir);
            }
            else{
                return;
            }

        }
        if(Input.GetKey("a")||Input.GetKey("w")||Input.GetKey("d")||Input.GetKey("s"))
        {
            _anim.SetTrigger("run"); //animation courir quand toucches pour bouger
        }
        if(_dx.x<0)
        {
            gameObject.transform.localScale = new Vector3(-1.5f,1.5f,1f); //flip le sprite horizontalement selon la direction
        }
        else{
            gameObject.transform.localScale = new Vector3(1.5f,1.5f,1f);
        }

        if(_energy>100||_energy<0)
        {
            if(_isOverloaded==false)
            InvokeRepeating("DamageBattery",2f,3f); //si l<energie est pas entre 100 et 1, il y a overload, qui damage le joueur de fa]on repetee
            _isOverloaded=true;
            _texteCharge.color=Color.red; //indication visuelle de l<etat d<overload, %energie devient rouge
        }
        else
        {
            _isOverloaded=false;
            CancelInvoke("DamageBattery");
            _texteCharge.color = Color.green;
        }

        if(Input.GetKeyDown("1"))
        {
            transform.position = _SpawnPoints[0].position;
        }
        else if(Input.GetKeyDown("2"))
        {
            transform.position = _SpawnPoints[1].position;
        }
        else if(Input.GetKeyDown("3"))
        {
            transform.position = _SpawnPoints[2].position;  //quand on appuie sur 1 2 3, le joueur se teleporte a divers endroits de la map
        }
        else if (Input.GetKeyDown("escape"))
        {
            _manager.Reset();
        }
        if(_vies<=0)
        {
            _manager.Reset();
        }
        else{
            return;
        }

    }

    private IEnumerator AddWallEnergy()
    {
        while (true)
        {
          yield return new WaitForSeconds(_tickTime); //au contact obstacle, ajoute l,energie du obstacle selon le ticktime
          _energy+=_energyWallPower;
        }
          
    }
    private IEnumerator LoseEnergyPerSec()
    {
        while (true)
        {
          yield return new WaitForSeconds(1f);
          _energy-=_energyLossPerSec;
        //   Debug.Log(_energy);
        }

    }

        private void DamageBattery()
    {
        EnleverVie(1);
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Obstacle"))
        {
            StartCoroutine("AddWallEnergy");
            _soundManager.JouerSon(_sonEnergyWall);
        }
        else if (other.CompareTag("Bullet"))
        {
            _energy+=_energyProjectilePower; //au lieu de prendre du damage, les projectiles ennemis sont convertis en energie
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Obstacle"))
        StopCoroutine("AddWallEnergy"); //quand le joueur sort de l<obstacle arrete d<ajouetr de l<enrgie
    }

    public void Reset()
    {
        transform.position = _SpawnPoints[0].position; //retourne au debut
        _vies= _VIES_MAX;
        _energy = 90;
        for (int i = 0; i < _heartContainers.Length; i++)
        {
            _heartContainers[i].SetActive(true); //redonne toutes les vies (et affichage)
        }
    }

    private void EnleverVie(int damage)
    {
        _vies-=damage;
        if(_vies<3)
        {
            _heartContainers[0].SetActive(false); //quand le joueur perd une vie, enleve le sprite correspondant
        }
        if(_vies<2)
        {
            _heartContainers[1].SetActive(false);
        }
        if(_vies<1)
        {
            _heartContainers[2].SetActive(false);
        }
        _soundManager.JouerSon(_sonViePerdue);
    }

    private void UpdateCharge()
    {
        _texteCharge.text = "Charge: "+_energy+"%";
        Debug.Log(_energy);
    }
}
