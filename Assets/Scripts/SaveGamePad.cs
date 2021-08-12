using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGamePad : MonoBehaviour
{
    public Player player;
    public Companion companion;
    public GameObject FloatingTextPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        SaveSystem.SaveGame(player, companion);

        ShowFloatingText();
    }

    public void ShowFloatingText()
    {
        Instantiate(FloatingTextPrefab, new Vector3(companion.transform.position.x, companion.transform.position.y + 2.5f, companion.transform.position.z) , Quaternion.identity, companion.transform);
    }

}
