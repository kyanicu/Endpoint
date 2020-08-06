using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DoorBehavior : MonoBehaviour
{
    public AudioClip OpenDoor;
    public AudioClip CloseDoor;
    public GameObject BottomDoor;
    public GameObject TopDoor;
    public float velocity;

    private float topStart;
    private float riseDistance;
    private float bottomStart;
    private float lowerDistance;

    private AudioSource audioSource;

    private bool open;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        topStart = TopDoor.transform.localPosition.y;
        bottomStart = BottomDoor.transform.localPosition.y;
        riseDistance = TopDoor.transform.localPosition.y + 1.0f;
        lowerDistance = BottomDoor.transform.localPosition.y - 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 topDoorPostion = TopDoor.transform.localPosition;
        Vector2 bottomDoorPosition = BottomDoor.transform.localPosition;
        if (open)
        {
            if (topDoorPostion.y < riseDistance)
            {
                TopDoor.transform.localPosition = new Vector2(topDoorPostion.x, topDoorPostion.y + velocity);
            }
            if (BottomDoor.transform.localPosition.y > lowerDistance)
            {
                BottomDoor.transform.localPosition = new Vector2(bottomDoorPosition.x, bottomDoorPosition.y - velocity);
            }
        }
        else
        {
            if (topDoorPostion.y > topStart)
            {
                TopDoor.transform.localPosition = new Vector2(topDoorPostion.x, topDoorPostion.y - velocity);
            }
            if (bottomDoorPosition.y < bottomStart)
            {
                BottomDoor.transform.localPosition = new Vector2(bottomDoorPosition.x, bottomDoorPosition.y + velocity);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            audioSource.clip = OpenDoor;
            audioSource.Play();
            open = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            open = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioSource.clip = CloseDoor;
            audioSource.Play();
            open = false;
        }
    }
}
