using UnityEngine;

namespace FreeFlowCombatSpace
{
    public class MovePlayer : MonoBehaviour
    {
        public float playerSpeed = 5f;

        CharacterController controller;
        Animator anim;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero) {
                gameObject.transform.forward = move;
                anim.SetBool("Run", true);
            }else{
                anim.SetBool("Run", false);
            }
        }
    }
}
