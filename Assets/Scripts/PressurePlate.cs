
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private AudioSource source;
    public AudioClip sound_plateEnabled;
    public AudioClip sound_plateDisabled;
    bool isOpen = false;

    int collionsObjCount = 0;
    public int triggerId1 = 0;
    public int triggerId2 = 0;

    Renderer pressurePlateRenderer;
    Vector3 initPos;
    float plateOffset;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Renderer component of the GameObject
        pressurePlateRenderer = GetComponent<Renderer>();
        source = gameObject.AddComponent<AudioSource>();
        pressurePlateRenderer.material.color = Color.red;
        initPos = gameObject.transform.position;
        plateOffset = 0.5f * pressurePlateRenderer.bounds.size.y;
    }
    // Update is called once per frame
    void Update()
    {
        {
            float step = 1f * Time.deltaTime; // calculate distance to move
            if (isOpen)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(initPos.x, initPos.y - plateOffset, initPos.z), step);
            }
            else
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, initPos, step);
            }


        }

    }

    private void ActivatePlate()
    {
        collionsObjCount++;
        isOpen = true;
        pressurePlateRenderer.material.color = Color.green;
        EventsManager.instance.OnPressurePlateEnable(triggerId1);
        EventsManager.instance.OnPressurePlateEnable(triggerId2);
        source.PlayOneShot(sound_plateEnabled);
    }

    private void DeactivatePlate()
    {
        collionsObjCount--;

        if (collionsObjCount == 0)
        {
            isOpen = false;
            pressurePlateRenderer.material.color = Color.red;
            EventsManager.instance.OnPressurePlateDisable(triggerId1);
            EventsManager.instance.OnPressurePlateDisable(triggerId2);
            source.PlayOneShot(sound_plateDisabled);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "GrabbableObject")
        {
            ActivatePlate();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "GrabbableObject")
        {
            DeactivatePlate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Companion")
        {
            ActivatePlate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Companion")
        {
            DeactivatePlate();
        }
    }

}
