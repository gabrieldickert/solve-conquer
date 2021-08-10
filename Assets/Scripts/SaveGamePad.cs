using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGamePad : MonoBehaviour
{
    public Player player;
    private bool isActive = false;
     
     private void OnTriggerExit(Collider other)
    {
        isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!isActive){
            SaveSystem.SavePlayer(player);
        }

        isActive = true;
    }
}
