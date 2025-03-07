using UnityEngine;

namespace FreeflowCombatSpace
{
    public class ExitGame : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                QuitGame();
            }
        }

        void QuitGame() 
        {
            #if UNITY_STANDALONE
                Application.Quit();
            #endif

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}

