using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {

    public float[] positionPlayer, positionCompanion;

    public GameData (Player player, Companion companion)
    {
        positionPlayer = new float[3];
        positionPlayer[0] = player.transform.position.x;
        positionPlayer[1] = player.transform.position.y;
        positionPlayer[2] = player.transform.position.z;

        positionCompanion = new float[3];
        positionCompanion[0] = companion.transform.position.x;
        positionCompanion[1] = companion.transform.position.y;
        positionCompanion[2] = companion.transform.position.z;
    }

}
