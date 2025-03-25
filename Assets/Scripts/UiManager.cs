using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private GameObject activeUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowUI(GameObject uiObject)
    {
        if (activeUI != null)
        {
            activeUI.SetActive(false);
        }

        activeUI = uiObject;
        activeUI.SetActive(true);
    }

    public void HideActiveUI()
    {
        if (activeUI != null)
        {
            activeUI.SetActive(false);
            activeUI = null;
        }
    }
}
