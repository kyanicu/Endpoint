using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
    private const float CAM_SPEED = 5;

    //Distance camera rests above player
    private const float yMod = .15f;

    //Camera's nonchanging z positioning
    private float zMod;

    private Transform player;
    private ParticleSystem[] hackSpirit;

    private void Start()
    {
        zMod = transform.position.z;
        hackSpirit = GetComponentsInChildren<ParticleSystem>();
    }

    void FixedUpdate()
    {
        if (PlayerController.instance == null)
        {
            return;
        }

        Transform player = PlayerController.instance.Character.gameObject.transform;
        float interpolation = CAM_SPEED * Time.deltaTime;

        Vector3 camPos = transform.position;
        float dist = Vector2.Distance(player.position, camPos);

        //Perform different type of camera movement dependent on if we're swapping or not
        if (InputManager.instance.currentState == InputManager.InputState.LOADING)
        {
            foreach(ParticleSystem ps in hackSpirit)
            {
                if (!ps.isPlaying)
                {
                    ps.Play();
                }
            }
            //If camera not near player pos
            if (dist > .1)
            {
                Vector2 updatePos = Vector2.MoveTowards(transform.position, player.position, interpolation);
                camPos.z = zMod;

                transform.position = new Vector3(updatePos.x, updatePos.y, zMod);
            }
            else
            {
                InputManager.instance.currentState = InputManager.InputState.GAMEPLAY;
                foreach (ParticleSystem ps in hackSpirit)
                {
                    ps.Stop();
                }
            }
        }
        //Otherwise, do normal camera movement
        else
        {
            //If camera not near player pos
            if (dist > .025)
            {
                //Lerp that ish
                camPos.x = Mathf.Lerp(transform.position.x, player.position.x, interpolation);
                camPos.y = Mathf.Lerp(transform.position.y + yMod, player.position.y + yMod, interpolation);
                camPos.z = zMod;

                transform.position = camPos;
            }
        }
    }
}