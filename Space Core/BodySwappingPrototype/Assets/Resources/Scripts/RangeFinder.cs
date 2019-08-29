using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangeFinder : MonoBehaviour
{
    private float uploadTime = 2;
    [SerializeField]
    private bool hackStart = false;
    public QTEManager qteManager;
    private SphereCollider sphere;
    public Image LoadingBar;

    // Start is called before the first frame update
    void Start()
    {
        sphere = GetComponent<SphereCollider>();
        LoadingBar.fillAmount = 0;
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
        while (LoadingBar.fillAmount < 1)
        {
            if (hackStart)
            {
                LoadingBar.fillAmount += Time.deltaTime/uploadTime;
            }
        }
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
