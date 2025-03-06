using UnityEngine;

namespace FreeflowCombatSpace
{
    public class SetInputs : MonoBehaviour
    {
        FreeflowCombat freeFlowCombat;
        public AudioSource whooshSound;
        public TrailRenderer tr;


        void Start()
        {
            freeFlowCombat = GetComponent<FreeflowCombat>();
        }
        

        void Update()
        {
            // left mouse click to attack
            if (Input.GetMouseButtonDown(0)) {
                freeFlowCombat.Attack();
            }


            // play woosh sound
            if (freeFlowCombat.isTraversing) {
                if (!whooshSound.isPlaying) whooshSound.Play();
            }
            else {
                whooshSound.Stop();
            }


            // enable the trail if either traversing or attacking
            if (freeFlowCombat.isTraversing || freeFlowCombat.isAttacking) tr.enabled = true;
            else tr.enabled = false;


            // SETTING THE INPUTS
            freeFlowCombat.xInput = Input.GetAxis("Horizontal");
            freeFlowCombat.yInput = Input.GetAxis("Vertical");
        }
    }
}

