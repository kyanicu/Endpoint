using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeFinder : MonoBehaviour
{
    private float uploadTime = 2;
    [SerializeField]
    private bool hackStart = false;
    public QTEManager qteManager;
    private SphereCollider sphere;

    // Start is called before the first frame update
    void Start()
    {
        sphere = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            hackStart = true;
            StartCoroutine(Uploading());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            hackStart = false;
            hideQTE();
        }
    }

    void startQTE()
    {
        qteManager.gameObject.SetActive(true);
        qteManager.StackCreate(false);
    }

    void hideQTE()
    {
        qteManager.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (hackStart == false) StopCoroutine(Uploading());
    }

    private IEnumerator Uploading()
    {
        yield return new WaitForSeconds(uploadTime);
        if(hackStart)
        {
            startQTE();
        }
        yield return null;
    }
}
