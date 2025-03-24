using System.Collections;
using UnityEngine;

public class TextIntro : MonoBehaviour
{
    public GameObject uiObject;

    void Start()
    {
        uiObject.SetActive(false);
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            UIManager.instance.ShowUI(uiObject);
            StartCoroutine(WaitForSec());
        }
    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(7);
        UIManager.instance.HideActiveUI();
        Destroy(gameObject);
    }
}