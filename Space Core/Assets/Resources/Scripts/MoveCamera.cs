using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
    private const float CAM_SPEED = 5;
    private const float CAM_ZOOM_OUT = 78.45135f;
    private const float CAM_ZOOM_IN = 35;
    private const int SPIRIT_ZOOM_COUNTER = 35;

    //Distance camera rests above player
    private const float yMod = .15f;

    //Camera's nonchanging z positioning
    private float zMod;
    private bool adjustingZoom;

    private Transform player;
    private ParticleSystem[] hackSpirit;
    private Camera camera;

    private void Start()
    {
        zMod = transform.position.z;
        hackSpirit = GetComponentsInChildren<ParticleSystem>();
        camera = GetComponent<Camera>();
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
            //Zoom in on first frame of hack swap
            if(!adjustingZoom && camera.fieldOfView == CAM_ZOOM_OUT)
            {
                adjustingZoom = false;
                StartCoroutine(adjustCameraZoom(true));
            }
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
                PlayerController.instance.Character.SetMeshEmissionColor(Color.red);

                StartCoroutine(adjustCameraZoom(false));
                adjustingZoom = true;
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

    /// <summary>
    /// Prevent hack particles from being visible when not swapping 
    /// </summary>
    private void LateUpdate()
    {
        //Verify that the player is in the game play state
        if (InputManager.instance.currentState == InputManager.InputState.GAMEPLAY)
        { 
            //Loop through each particle system in "hack spirit"
            foreach (ParticleSystem ps in hackSpirit)
            {
                //If the particle system is playing, stop it
                if (ps.isPlaying)
                {
                    ps.Stop();
                }
            }
        }
    }

    private IEnumerator adjustCameraZoom(bool zoomingIn)
    {
        float zoomAdjustAmt = (CAM_ZOOM_OUT - CAM_ZOOM_IN) / SPIRIT_ZOOM_COUNTER;
        int counter = 0;

        //If zoomingIn, multiply zoomAdjustAmt by -1 so FoV decreases
        int zoomMod = zoomingIn ? -1 : 1;

        while(counter < SPIRIT_ZOOM_COUNTER)
        {
            camera.fieldOfView += zoomAdjustAmt * zoomMod;
            counter++;
            yield return null;
        }
    }
}