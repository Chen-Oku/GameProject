

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : MonoBehaviour
{
    Animator animator;
    PlayerControllerSak playerControllerSak;
    int isWalkingHash;
    int isRunningHash;
    int isAttackingHash;
    int isJumpingHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerControllerSak = GetComponent<PlayerControllerSak>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isAttackingHash = Animator.StringToHash("isAttacking");
        isJumpingHash = Animator.StringToHash("isJumping");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w") || Input.GetKey("s");
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool runPressed = Input.GetKey("left shift");
        bool attackPressed = Input.GetMouseButton(0);
        bool jumpPressed = Input.GetKeyDown("space");

        bool movementKeyPressed = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        ////Si el jugador presiona el bot n de ataque
        //if (attackPressed)
        //{
        //    animator.SetBool(isAttackingHash, true);
        //    animator.SetBool(isWalkingHash, false);
        //    animator.SetBool(isRunningHash, false);

        //}
        //else
        //{
        //    if (!attackPressed)
        //    {
        //        animator.SetBool(isAttackingHash, false);
        //    }

        if (Input.GetMouseButtonDown(0)) // Botón izquierdo del ratón para atacar
        {
            animator.SetBool(isAttackingHash, true);
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isRunningHash, false);
        }
        else
        {
            if (!attackPressed)
            {
                animator.SetBool(isAttackingHash, false);
            }
        }

        //Si el jugador presiona una tecla de movimiento
        if (!isWalking && movementKeyPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else
        {
            // Si el jugador no est  presionando una tecla de movimiento
            if (isWalking && !movementKeyPressed)
            {
                animator.SetBool(isWalkingHash, false);
            }
        else
                {
            // Si el jugador no est  presionando una tecla de movimiento
            if (movementKeyPressed && jumpPressed)
            {
                animator.SetBool(isWalkingHash, false);
                animator.SetBool(isJumpingHash, true);
            }
        }
        }

        // Si el jugador est  caminando y presiona la tecla de correr
        if (movementKeyPressed  && runPressed)
        {
            animator.SetBool(isRunningHash, true);
        }
        else
        {
            // Si el jugador no est  presionando la tecla de correr
            if (!movementKeyPressed || !runPressed)
            {
                animator.SetBool(isRunningHash, false);
            }
        }
                //Si el jugador presiona la tecla de salto o la tecla de salto y una tecla de movimiento
                if (jumpPressed || (jumpPressed && movementKeyPressed))
                    {
                        animator.SetBool(isJumpingHash, true);               
                    }
                    else
                    {
                        // Si el jugador no est  presionando la tecla de salto
                        if (!jumpPressed)
                        {
                            animator.SetBool(isJumpingHash, false);
                        }


                }

                
        /*
        if (jumpPressed)
        {
            animator.SetBool(isJumpingHash, true);
            if (playerControllerSak != null)
            {
                playerControllerSak.Jump();
            }
            
        }
        else
        {
            animator.SetBool(isJumpingHash, false);
        }

        // Si el jugador presiona la tecla de salto y una tecla de movimiento
        if (jumpPressed || (jumpPressed && forwardPressed))
        {
            animator.SetBool(isJumpingHash, true);
        }
        */
    }
    
}



/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateSak : MonoBehaviour
{
    Animator animator;
    PlayerControllerSak playerControllerSak;
    int isWalkingHash;
    int isRunningHash;
    int isAttackingHash;
    int isJumpingHash;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerControllerSak = GetComponent<PlayerControllerSak>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isAttackingHash = Animator.StringToHash("isAttacking");
        isJumpingHash = Animator.StringToHash("isJumping");
    }

    void Update()
    {
        HandleMovementAnimations();
        HandleAttackAnimation();
        HandleJumpAnimation();
    }

    private void HandleMovementAnimations()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool movementKeyPressed = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        animator.SetBool(isWalkingHash, movementKeyPressed);
        animator.SetBool(isRunningHash, movementKeyPressed && isRunning);
    }

    private void HandleAttackAnimation()
    {
        if (Input.GetMouseButtonDown(0)) // Botón izquierdo del ratón para atacar
        {
            animator.SetTrigger(isAttackingHash);
        }
    }

    private void HandleJumpAnimation()
    {
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger(isJumpingHash);
            if (playerControllerSak != null)
            {
                playerControllerSak.Jump();
            }
            else
            {
                Debug.LogError("PlayerControllerSak no está asignado.");
            }
        }
    }
}

*/