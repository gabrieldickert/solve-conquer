using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public GameObject respawnablesParentObject;
    public AudioClip sound_objectRespawned;
    
    private AudioSource source;
    private List<GameObject> respawningObjectsList = new List<GameObject>();

    private Dictionary<int, GameObjectInit> initialPositions = new Dictionary<int, GameObjectInit>();

    void Start()
    {
        EventsManager.instance.ResetObject += HandleResetObject;
        this.source = gameObject.AddComponent<AudioSource>();
        FillList();
        SaveInitialPositions();
    }

    void Update()
    {
        
    }

    void FillList()
    {
        Transform[] allChildren = respawnablesParentObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            this.respawningObjectsList.Add(child.gameObject);
        }

    }

    void SaveInitialPositions()
    {
        foreach (GameObject go in respawningObjectsList)
        {
            this.initialPositions.Add(go.GetInstanceID(), new GameObjectInit(go));
            
        }
    }

    void HandleResetObject(int instanceId)
    {
        this.initialPositions[instanceId].gameObject.transform.position = this.initialPositions[instanceId].initialPosition;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        int instanceId = other.gameObject.GetInstanceID();
        
        if (this.initialPositions.ContainsKey(instanceId))
        {
            if(other.gameObject.tag != "Companion")
            {
                this.initialPositions[instanceId].gameObject.transform.position = this.initialPositions[instanceId].initialPosition;
            } else
            {
                other.gameObject.transform.parent.GetComponent<NavMeshAgent>().transform.position = this.initialPositions[instanceId].initialPosition;
                other.gameObject.transform.parent.GetComponent<NavMeshAgent>().enabled = false;
                other.gameObject.transform.parent.GetComponent<NavMeshAgent>().enabled = true;
            }
            this.source.PlayOneShot(sound_objectRespawned);

        }
        else if (other.gameObject.tag == "Player")
        {
            // reset scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            this.source.PlayOneShot(sound_objectRespawned);
        }
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
