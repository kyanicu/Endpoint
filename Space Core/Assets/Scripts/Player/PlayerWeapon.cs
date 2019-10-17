using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Weapon gun;
    public GameObject RotationPoint;

    private void Start()
    {
        RotationPoint = transform.Find("RotationPoint").gameObject;
        gun = WeaponGenerator.GenerateWeapon(RotationPoint.transform.Find("WeaponLocation")).GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(RotationPoint.transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(mouseOnScreen, positionOnScreen);

        if (mouseOnScreen.x < positionOnScreen.x)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f));
            RotationPoint.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, angle));
        }
        else
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f));
            RotationPoint.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }

        if (Input.GetMouseButton(0))
        {
            gun.Fire();
            Debug.Log($"Number of shots left: {gun.AmmoInClip}");
            Debug.Log($"Total Ammo: {gun.TotalAmmo}");
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log($"Reloading!");
            StartCoroutine(gun.Reload());
            Debug.Log($"Number of shots left: {gun.AmmoInClip}");
            Debug.Log($"Total Ammo: {gun.TotalAmmo}");
        }
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
