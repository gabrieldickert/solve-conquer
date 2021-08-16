using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {

    public float[] positionPlayer, positionCompanion, positionPad;
    public string stage;
    public string lvl;

    public GameData (Player player, Companion companion, SaveGamePad pad,string stage,string lvl)
    {
        positionPlayer = new float[3];
        positionPlayer[0] = player.transform.position.x;
        positionPlayer[1] = player.transform.position.y;
        positionPlayer[2] = player.transform.position.z;

        positionCompanion = new float[3];
        positionCompanion[0] = companion.transform.position.x;
        positionCompanion[1] = companion.transform.position.y;
        positionCompanion[2] = companion.transform.position.z;

        positionPad = new float[3];
        positionPad[0] = pad.transform.position.x;
        positionPad[1] = pad.transform.position.y;
        positionPad[2] = pad.transform.position.z;

        this.stage = stage;
        this.lvl = lvl;
    }

}
