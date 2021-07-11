using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public List<GameObject> respawningObjectsList = new List<GameObject>();
    private Dictionary<int, GameObjectInit> initialPositions = new Dictionary<int, GameObjectInit>();

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.instance.ResetObject += HandleResetObject;
        //UnityEngine.Debug.Log("RespawnManager: start");
        SaveInitialPositions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SaveInitialPositions()
    {
        //UnityEngine.Debug.Log("RespawnManager: Saving initial positions");
        foreach (GameObject go in respawningObjectsList)
        {
            UnityEngine.Debug.Log("RespawnManager: Added object id " + go.GetInstanceID() + " to dictionary");
            this.initialPositions.Add(go.GetInstanceID(), new GameObjectInit(go));
        }
    }

    void HandleResetObject(int instanceId)
    {
        this.initialPositions[instanceId].gameObject.transform.position = this.initialPositions[instanceId].initialPosition;
    }

}

public class GameObjectInit
{
    public GameObject gameObject { get; set; }
    public Vector3 initialPosition { get; set; }

    public GameObjectInit(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.initialPosition = this.gameObject.transform.position;
    }
}
