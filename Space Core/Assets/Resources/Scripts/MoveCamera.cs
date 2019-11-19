using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
    //Lerp speed of camera
    public float speed = 2000000f;
    private const float yMod = .15f;

    void Update()
    {
        Transform player = Player.instance.gameObject.transform;
        float interpolation = speed * Time.deltaTime;

        Vector3 position = transform.position;
        if (position != new Vector3(player.position.x, player.position.y + yMod, transform.position.z))
        {
        position.y = Mathf.Lerp(transform.position.y + yMod, player.position.y + yMod, interpolation);
        position.x = Mathf.Lerp(transform.position.x, player.position.x, interpolation);

        transform.position = position;
        }
    }
}