using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
    //Lerp speed of camera
    public float speed = 2.0f;

    void Update()
    {
        Transform player = Player.instance.gameObject.transform;
        float interpolation = speed * Time.deltaTime;

        Vector3 position = transform.position;
        position.y = Mathf.Lerp(transform.position.y, player.position.y, interpolation);
        position.x = Mathf.Lerp(transform.position.x, player.position.x, interpolation);

        transform.position = position;
    }
}