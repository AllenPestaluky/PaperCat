using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCollisionManager : MonoBehaviour
{
    CatDeath catDeath;

    void Awake()
    {
        catDeath = GetComponent<CatDeath>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TriggerZone")
        {
            other.SendMessage("Trigger");
        }

        if (other.tag == "Checkpoint")
        {
            catDeath.UpdateCheckpoint(other.transform);   
            other.SendMessage("Trigger");
        }
        
        if (other.tag == "Deadly")
        {
            catDeath.StartDying();
        }
    }
}
