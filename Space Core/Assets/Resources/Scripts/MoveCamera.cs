using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
    //Lerp speed of camera
    public float speed = 5;

    //Distance camera rests above player
    private const float yMod = .15f;

    //Camera's nonchanging z positioning
    private float zMod;

    private void Start()
    {
        zMod = transform.position.z;
    }

    void FixedUpdate()
    {
        Transform player = Player.instance.gameObject.transform;
        float interpolation = speed * Time.deltaTime;

        Vector3 camPos = transform.position;

        //If camera not near player pos
        if (!Mathf.Approximately(player.position.x, camPos.x) &&
            !Mathf.Approximately(player.position.y, camPos.y))
        {
            //Lerp that ish
            camPos.y = Mathf.Lerp(transform.position.y + yMod, player.position.y + yMod, interpolation);
            camPos.x = Mathf.Lerp(transform.position.x, player.position.x, interpolation);
            camPos.z = zMod;

            transform.position = camPos;
        }
    }
}