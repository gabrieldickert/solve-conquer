using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGamePad : MonoBehaviour
{
    public Player player;
    public Companion companion;
     
    private void OnTriggerEnter(Collider other)
    {
            SaveSystem.SaveGame(player, companion);
    }
}
