using UnityEngine;

namespace FreeflowCombatSpace
{
    public class MoveCamera : MonoBehaviour
    {
        public Transform camera;

        void LateUpdate() 
        {
            if (camera) camera.position = new Vector3(transform.position.x, camera.position.y, transform.position.z - 8f);    
        }
    }
}

