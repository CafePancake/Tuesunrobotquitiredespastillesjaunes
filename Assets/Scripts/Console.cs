using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _CUnits; //spriterenderer des objects a activer en ordre
    [SerializeField] private CodeSnip[] _codeSnips;   //FOR THE LOVE OF GOD PLEASE RENAME CODE RELATED VARIABLES
    [SerializeField] private SpriteRenderer[] _clues; //sprite renderer des objets qui ont la r/ponse du puzzle (en haut de la salle 1)
    [SerializeField] private Sprite[] _symbols; //sprites des symboles inputs
    [SerializeField] private GameObject _door;//porte qui s<ouvre quand le joueur reussi le puzzle
    [SerializeField] private SoundManager _soundManager; //manager du son
    [SerializeField] private AudioClip _sonCorrect; //son quand reussi le puzzle
    [SerializeField] private AudioClip _sonIncorrect; //son quand fail le puzzle
    [SerializeField] private AudioClip _sonPorte; //son de la porte qui ouvre

    private List<int> _codeNums = new List<int>{0, 1, 2}; //les numeros qui composent le code
    private string _code = ""; //le code
    private string _input = ""; //le code input par le joueur
    private int _inputCount; //le nombre de inpupts faits par le joueur
    void Start()
    {
        GenerateCode();
    }

    private void GenerateCode()
    {
        for (int i = 0; i < _CUnits.Length; i++)
        {
            int _codeDigit = _codeNums[Random.Range(0,_codeNums.Count)]; //prends un chiffre aleatoire de la liste
            _clues[i].sprite=_symbols[_codeDigit]; //les indices ont un sprite qui equivaut a l<input du code correspondant ex code 012 indices crane etoile eclair
            _code+=_codeDigit; //le chiffre est ajout/ au code
            _codeNums.Remove(_codeDigit); //enleve le chiffre de la liste
        }
    }

    public void Input(string number)
    {
        _input +=number; //number est le nom de l<object active (0, 1 ou 2)
        _inputCount++;
        if(_inputCount==_CUnits.Length)
        {
            Verify(); //quand tous le nombre d<inputs est egal au nombre d<object a activer verifier el code input
        }
    }

    private void Verify()
    {
        if(_code==_input)
        {
            _soundManager.JouerSon(_sonCorrect);
            _door.SetActive(false); //si le bon code desactive la porte
            _soundManager.JouerSon(_sonPorte);
        }
        else{
            _soundManager.JouerSon(_sonIncorrect);
            for (int i = 0; i < _CUnits.Length; i++)
            {
                Reset();
                _codeSnips[i].Reset(); //sinon reset le puzzle
                // Debug.Log(i);
            }
        }
    }

    public void Reset()
    {
        _code="";
        _input= "";
        _inputCount=0;
        _codeNums = new List<int>{0, 1, 2}; //reset et genere un nouveau code
        GenerateCode();
    }

}
