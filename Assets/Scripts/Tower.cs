using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameObject _Player; //le personnage controle par le joueur
    [SerializeField] private GameObject _missile; //prefab projectile qui suit le joueur
    [SerializeField] private GameObject _bullet; //prefeb projectile standard
    [SerializeField] private GameObject _healthbar; //barre de vie (en rouge)
    private SpriteRenderer _renderer; //sprite renderer du boss
    [SerializeField] private GameManager _manager; //game manager
    [SerializeField] private SoundManager _soundManager; //sound manager
    [SerializeField] private AudioClip _sontir; //son quand le boss tire un projectile (desactive pour bruit exessif)
    [SerializeField] private AudioClip _sonMort; //son quand le boss meurt (desactive pour bug)

    private Vector3 _spawn = new Vector3(7,6,0); //coordonees ou le boss apparait quand on reset
    private float _vitesse = 2f; //vitesse du boss
    private string _nextAttack; //nom de lattaque qui suivra 
    private int _spread; //repartition des projectiles
    private int _bulletCount; //nombre de balles tirees
    private int _bulletLimit; //max nombre de balles tirees durant lattaque 
    private bool _isAwake; //determine si le boss est actif ou non
    private int _hp = 25; //nombre de vies du boss
    // Start is called before the first frame update
    void Start()
    {
        _isAwake=false; //le boss n<est pas actif au debut
        _renderer=GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_isAwake)
        {
            transform.position = Vector2.MoveTowards(transform.position, _Player.transform.position, _vitesse * Time.deltaTime); //se d/place si actif
        }

        if(_hp<=0)
        {
            Die();
        }

    }

    public void WakeUp()
    {
        _isAwake = true;
        Invoke("BulletRain", 2f); //sactive quand le joueur pass le checkpoint3
    }

    private void BulletRain()
    {
        _spread = 4;
        _bulletLimit = 40;
        _nextAttack = "BulletStorm";
        InvokeRepeating("ShootBullet", 0f, 0.2f);           //bulletrain, bulletstorm et bullethell wont les trois patterns dattaques
                                                            //chaque pattern a un nombre de balles max predefini, un spread, cadence
    }
    private void BulletStorm()
    {   
        _spread = 30;
        _bulletLimit = 80;
        _nextAttack = "BulletHell";                         //next attack est decide a chaque attaque, dans ce cas-ci les 3 se suivent en ordre
        InvokeRepeating("ShootBullet",0f, 0.1f);
    }
    private void BulletHell()
    {
        _spread = 90;
        _bulletLimit = 160;
        _nextAttack = "BulletRain";
        InvokeRepeating("ShootBullet", 0f, 0.05f);
    }
    private void ShootBullet()
    {
        Instantiate(_bullet, transform.position, Quaternion.Euler(0f,0f, Random.Range(-90 -_spread,-90+_spread)));      //fonction qui s<occupe de tirer, instancie _bullet
        _bulletCount+=1;      
        if(_bulletCount>=_bulletLimit) //la fonction compte les balles, arrete l<attaque quand le max est atteint et invoque la prochaine
        {
            CancelInvoke();
            _bulletCount=0;
            Invoke(_nextAttack,0f);
        }

        int missileCoinFlip = Random.Range(0,51); //le boss tire parfois des projectiles qui suivent 
        if(missileCoinFlip==1)
        {
            ShootMissile();
        }
        // _soundManager.JouerSon(_sontir);
        // Debug.Log(_bulletCount);
    }

    private void ShootMissile()
    {
        Instantiate(_missile, transform.position, Quaternion.identity); 
    }

    public void reset()
    {
        transform.position = _spawn;
        _isAwake=false;
        CancelInvoke();
        _healthbar.transform.localScale=new Vector3(1f,0.1f,0f); //le boss retourne a son spawn, s<eteint et son hp est reinit.
        _hp = 25;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Energy"))
        {
            if(_isAwake==true)
            {
                _hp-=1;
                _healthbar.transform.localScale-=new Vector3(0.04f,0f,0f); //au contact d<un projectile le boss perds 1/25 de sa barre de vie
            }

        }
    }
    private void Die()
    {
        // _soundManager.JouerSon(_sonMort);
        _isAwake=false;
        _renderer.color = Color.grey;
        CancelInvoke();
        Invoke("EndGame", 1f);
    }

    private void EndGame()
    {
        _manager.Victoire(); //demande au manager de jouer l,ecran de fin
    }
}
