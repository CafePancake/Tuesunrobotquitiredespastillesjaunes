using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint3 : MonoBehaviour
{
    [SerializeField] private Tower _boss; //script du boss

    void OnTriggerEnter2D(Collider2D other)
    {
        _boss.WakeUp(); //active le boss quand quelquechose passe
    }
}
