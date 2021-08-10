using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Companion companion;

    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();

        Vector3 positionPlayer;
        positionPlayer.x = data.positionPlayer[0];
        positionPlayer.y = data.positionPlayer[1];
        positionPlayer.z = data.positionPlayer[2];
        this.transform.position = positionPlayer;

        Vector3 positionCompanion;
        positionCompanion.x = data.positionCompanion[0];
        positionCompanion.y = data.positionCompanion[1];
        positionCompanion.z = data.positionCompanion[2];
        companion.transform.position = positionCompanion;
    }
}
