using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private AudioSource source;
    public AudioClip sound_plateEnabled;
    public AudioClip sound_plateDisabled;
    private bool isOpen = false;

    private int collionsObjCount = 0;
    public int triggerId;

    private Renderer pressurePlateRenderer;
    private Vector3 initPos;
    private float plateOffset;

    // Start is called before the first frame update
    private void Start()
    {
        //Fetch the Renderer component of the GameObject
        pressurePlateRenderer = GetComponent<Renderer>();
        source = gameObject.AddComponent<AudioSource>();
        pressurePlateRenderer.material.color = Color.red;
        initPos = gameObject.transform.position;
        plateOffset = 0.5f * pressurePlateRenderer.bounds.size.y;
    }

    // Update is called once per frame
    private void Update()
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

    private void OnCollisionEnter(Collision collision)
    {
        collionsObjCount++;
        isOpen = true;
        pressurePlateRenderer.material.color = Color.green;
        EventsManager.instance.OnPressurePlateEnable(triggerId);
        source.PlayOneShot(sound_plateEnabled);
    }

    private void OnCollisionExit(Collision collision)
    {
        collionsObjCount--;

        if (collionsObjCount == 0)
        {
            isOpen = false;
            pressurePlateRenderer.material.color = Color.red;
            EventsManager.instance.OnPressurePlateDisable(triggerId);
            source.PlayOneShot(sound_plateDisabled);
        }
    }
}