using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMesh Pro
using DG.Tweening; // Tweening Library (smooth animations/transitions)
using UnityEngine.UI;
using System.Linq;

public class WorldspaceCanvas : MonoBehaviour
{

    private static WorldspaceCanvas _instance;
    public static WorldspaceCanvas instance { get { return _instance; } }

    private PlayerCanvasManager PlayerCM;

    private void Awake()
    {
        //Setup singleton
        if (_instance == null)
        {
            _instance = this;
        }
    }

    // Updates the player canvas object to the canvas on the character the player is currently using.
    public void UpdateWorldspaceCanvas(Canvas WorldspaceCanvas)
    {
        PlayerCM = WorldspaceCanvas.GetComponent<PlayerCanvasManager>();
        PlayerCM.ammoText.DOFade(0f, 0f);
    }

    private Tween tweenAmmoText;

    /// Updates text above player when they recieve ammo.
    public void UpdatePlayerAmmo()
    {
        tweenAmmoText = PlayerCM.ammoText.DOFade(1f, 0f);
        tweenAmmoText = PlayerCM.ammoText.DOFade(0f, 1f);
    }

}