using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangeFinder : MonoBehaviour
{ 
    //Loading bar depicting how much hack time is left
    public Image LoadingBar;

    //Quick time event panel, activated when in range for long enough
    public QTEManager QTEManager;

    public GameObject QTEPanel;

    //Flag denoting whether player is currently uploading
    private bool hackStart = false;

    //Time it takes to upload, used in coroutine
    private float uploadTime = 1f;

    //How much fill amount gets increased 
    private const float UPLOAD_BAR_AMT = .0625f; 

    public int QTEButtonsAmt;

    /// <summary>
    /// Begins uploading upon player entering hack area
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
       if (col.gameObject.tag == "Player")
        {
            hackStart = true;

            LoadingBar.gameObject.SetActive(true);
            StartCoroutine(Uploading());
        }
    }

    /// <summary>
    /// Halts uploading upon player leaving hack area
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            hackStart = false;
            StopCoroutine(Uploading());
            QTEButtonsAmt = QTEManager.getButtonsLeft();
            LoadingBar.gameObject.SetActive(false);
            QTEPanel.gameObject.SetActive(false);
            LoadingBar.fillAmount = 0;
        }
    }

    /// <summary>
    /// Activate the qte panel and populate it with new buttons
    /// </summary>
    private void startQTE()
    {
        if (hackStart)
        {
            QTEPanel.gameObject.SetActive(true);
            QTEManager.onActivate(QTEButtonsAmt);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Uploading()
    {
        StartCoroutine(UpdateBar());
        yield return new WaitForSeconds(uploadTime);
        if(hackStart)
        {
            startQTE();
        }
        yield return null;
    }

    /// <summary>
    /// continues filling bar while player is in hack area
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateBar()
    {
        while(hackStart)
        {
            //Fill bar 1/16th of the way 
            LoadingBar.fillAmount += UPLOAD_BAR_AMT;

            //Wait 1/16th of the upload time before updating bar again
            yield return new WaitForSeconds(uploadTime / 16);
        }
        yield return null;
    }
}
