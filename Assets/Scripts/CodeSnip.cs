using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeSnip : MonoBehaviour
{
    private SpriteRenderer _renderer; //spriterenderer du gameobject
    [SerializeField] private SoundManager _soundManager; //manager du son
    
    [SerializeField] private AudioClip _sonActivation; //son quand le joueur tire sur l<objet
    [SerializeField] private Console _console; //object qui manage le puzzle
    private bool _ispressed; //bool si l<object a ete activ/
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = Color.grey;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Energy")){
            if(!_ispressed)
            {
                TurnOn(); //s<active quand le joueur tire dessus
            }
            else{
                return;
            }

        }
    }

    private void TurnOn()
    {
        _renderer.color=Color.white;
        _ispressed=true;
        _console.Input(gameObject.name); //envoi le nom du gameobject comme valeur quand le manager du puzzle
        //soundclic
    }

    public void Reset()
    {
        _renderer.color = Color.grey;
        _ispressed=false;
        //sound puzzlefail
    }
    
}
