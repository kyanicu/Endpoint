using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupManager : MonoBehaviour
{
    [Header("Objectives Popup")]
    public GameObject ObjectivesGroup;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI ObjectiveContent;
    private RectTransform ObjectivesRectTrans;

    [Header("Database Popup")]
    public GameObject DatabaseGroup;
    private RectTransform DatabaseRectTrans;

    #region Constants
    private const float popupDuration = 10f;
    private const float ObjPopupHiddenX = -414;
    private const float ObjPopupVisibleX = 206;
    private const float DBPopupHiddenX = -357;
    private const float DBPopupVisibleX = 150;
    private const float animationMoveSpeed = 7.5f;
    #endregion

    private void Start()
    {
        ObjectivesRectTrans = ObjectivesGroup.GetComponent<RectTransform>();
        DatabaseRectTrans = DatabaseGroup.GetComponent<RectTransform>();
    }

    /// <summary>
    /// Opens pop up and populate content with given arguments
    /// </summary>
    /// <param name="title"></param>
    /// <param name="content"></param>
    //HUDController.instance.InitiateObjectivesPopup("This is the title", "This is the content. It worked!");
    public void InitiateObjectivesPopup(string title, string content)
    {
        Title.text = title;
        ObjectiveContent.text = content;
        StartCoroutine(openObjectivesPopup());
    }

    /// <summary>
    /// Coroutine to wait and then hide objectives popup
    /// </summary>
    /// <returns></returns>
    private IEnumerator closeObjectivesPopup()
    {
        yield return new WaitForSeconds(popupDuration);
        Vector3 pos = ObjectivesRectTrans.position;
        while (pos.x > ObjPopupHiddenX)
        {
            pos = ObjectivesRectTrans.position;
            ObjectivesRectTrans.position = new Vector3(pos.x - animationMoveSpeed, pos.y, pos.z);
            yield return null;
        }
    }

    /// <summary>
    /// Coroutine to wait and then hide objectives popup
    /// </summary>
    /// <returns></returns>
    private IEnumerator openObjectivesPopup()
    {
        Vector3 pos = ObjectivesRectTrans.position;
        while (pos.x < ObjPopupVisibleX)
        {
            pos = ObjectivesRectTrans.position;
            ObjectivesRectTrans.position = new Vector3(pos.x + animationMoveSpeed, pos.y, pos.z);
            yield return null;
        }
        StartCoroutine(closeObjectivesPopup());
    }

    /// <summary>
    /// Opens pop up and populate content with given arguments
    /// </summary>
    /// <param name="title"></param>
    /// <param name="content"></param>
    public void InitiateDatabasePopup()
    {
        StartCoroutine(openDatabasePopup());
    }


    /// <summary>
    /// Close database popup without wait time
    /// </summary>
    public void CloseDBPopup()
    {
        HUDController.instance.RecentDataBaseEntry = null;
        Vector3 pos = DatabaseRectTrans.position;
        DatabaseRectTrans.position = new Vector3(DBPopupHiddenX, pos.y, pos.z);
    }

    /// <summary>
    /// Coroutine to unhide database popup 
    /// </summary>
    /// <returns></returns>
    private IEnumerator openDatabasePopup()
    {
        Vector3 pos = DatabaseRectTrans.position;
        while (pos.x < DBPopupVisibleX)
        {
            pos = DatabaseRectTrans.position;
            DatabaseRectTrans.position = new Vector3(pos.x + animationMoveSpeed, pos.y, pos.z);
            yield return null;
        }

        //Start coroutine to close it
        StartCoroutine(closeDatabasePopup());
    }

    /// <summary>
    /// Coroutine to wait and then hide database popup if it hasn't been manually
    /// close already
    /// </summary>
    /// <returns></returns>
    private IEnumerator closeDatabasePopup()
    {
        yield return new WaitForSeconds(popupDuration);
        Vector3 pos = DatabaseRectTrans.position;
        while (pos.x > DBPopupHiddenX)
        {
            //Update popup position
            pos = DatabaseRectTrans.position;
            DatabaseRectTrans.position = new Vector3(pos.x - animationMoveSpeed, pos.y, pos.z);
            yield return null;
        }

        //If HUD popup cache isn't empty, clear it
        if (HUDController.instance.RecentDataBaseEntry != null)
        {
            HUDController.instance.RecentDataBaseEntry = null;
        }
    }
}
