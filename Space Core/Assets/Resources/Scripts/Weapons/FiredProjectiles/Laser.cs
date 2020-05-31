using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Laser : MonoBehaviour
{
    public GameObject beamStart;
    public GameObject beamEnd;
    public GameObject beam;
    private LineRenderer line;

    [Header("Adjustable Variables")]
    public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3; //Length of the beam texture


    private void Start()
    {
        line = beam.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            Vector3 tdir = hit.point - transform.position;
            ShootBeamInDir(transform.position, tdir);
        }
    }

    void ShootBeamInDir(Vector2 start, Vector2 dir)
    {
        line.positionCount = 2;
        line.SetPosition(0, start);
        beamStart.transform.position = start;

        Vector2 end = Vector3.zero;
        RaycastHit2D hit = Physics2D.Raycast(start, dir);
        if (hit)
            end = hit.point - (dir.normalized * beamEndOffset);
        else
            end = new Vector2(transform.position.x, transform.position.y) + (dir * 100);

        beamEnd.transform.position = end;
        line.SetPosition(1, end);

        beamStart.transform.LookAt(beamEnd.transform.position);
        beamEnd.transform.LookAt(beamStart.transform.position);

        float distance = Vector2.Distance(start, end);
        line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
        line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }
}
