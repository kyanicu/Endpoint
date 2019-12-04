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

    [Header("Database Popup")]
    public GameObject DatabaseGroup;

    private const float popupDuration = 10f;

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
        StartCoroutine(closeObjectivesPopup());
    }

    /// <summary>
    /// Opens pop up and populate content with given arguments
    /// </summary>
    /// <param name="title"></param>
    /// <param name="content"></param>
    public void InitiateDatabasePopup()
    {
        StartCoroutine(closeDatabasePopup());
    }

    private IEnumerator closeObjectivesPopup()
    {
        yield return new WaitForSeconds(popupDuration);
        HUDController.instance.ObjectivesPopupIsActive = false;

        //TODO - animate popup closing instead of just deactivating game object
        ObjectivesGroup.SetActive(false);
    }

    private IEnumerator closeDatabasePopup()
    {
        yield return new WaitForSeconds(popupDuration);
        HUDController.instance.RecentDataBaseEntry = null;

        //TODO - animate popup closing instead of just deactivating game object
        DatabaseGroup.SetActive(false);
    }
}
