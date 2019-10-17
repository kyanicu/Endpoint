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

    //Flag denoting whether player is currently uploading
    private bool hackStart = false;

    //Time it takes to upload, used in coroutine
    private float uploadTime = 2;

    //How much fill amount gets increased 
    private const float UPLOAD_BAR_AMT = .0625f; 

    public int QTEButtonsAmt;

    /// <summary>
    /// Begins uploading upon player entering hack area
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            hackStart = true;
            StartCoroutine(Uploading());
        }
    }

    /// <summary>
    /// Halts uploading upon player leaving hack area
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            hackStart = false;
            StopCoroutine(Uploading());
            hideQTE();
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
            QTEManager.gameObject.SetActive(true);
            QTEManager.onActivate(QTEButtonsAmt);
        }
    }

    /// <summary>
    /// Hide the qte panel
    /// </summary>
    private void hideQTE()
    {
        QTEButtonsAmt = QTEManager.getButtonsLeft();
        QTEManager.gameObject.SetActive(false);
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
