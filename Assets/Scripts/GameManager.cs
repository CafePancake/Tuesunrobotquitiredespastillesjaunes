using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Tower _boss; //gameobject du boss
    [SerializeField] private GameObject _door; //gameobject de la porte
    [SerializeField] private Chara _joueur; //script du joueur
    [SerializeField] private Console _puzzle; //gameobject qui manage le puzzle

    public void Reset()
    {
        _joueur.Reset();
        _boss.reset();
        _door.SetActive(true); //demande au divers objects de reset et r/active la porte
        _puzzle.Reset();
    }

    public void JouerScene(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0); //scene du menu principal
    }

    public void Jouer()
    {
        SceneManager.LoadScene(1); //scene du jeu
    }

    public void Victoire()
    {
        SceneManager.LoadScene(2); //scene de fin
    }
    
}