using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGamePad : MonoBehaviour
{
    public Player player;
    public Companion companion;
    public GameObject FloatingTextPrefab;
    public Transform centerEyeAnchor;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            //Speichert erst wenn kein FloatingText(Clone) gefunden wird, d.h. alle 3 sec ist es möglich neu zu speichern (Zeit ist einstellbar im FloatingTexts script)
            //Verhindert das spammen der Speichern Funktion und der zugehörigen Nachricht
            if(centerEyeAnchor.Find("FloatingText(Clone)") == null)
            {
                SaveSystem.SaveGame(player, companion);
                ShowFloatingText();
            }
         
        }
    }

    public void ShowFloatingText()
    {
        //Instantiate(FloatingTextPrefab, new Vector3(companion.transform.position.x, companion.transform.position.y + 2.5f, companion.transform.position.z) , Quaternion.identity, companion.transform);
        Instantiate(FloatingTextPrefab, new Vector3(centerEyeAnchor.transform.position.x, centerEyeAnchor.transform.position.y, centerEyeAnchor.transform.position.z + 5), Quaternion.identity, centerEyeAnchor.transform);
    }

}
