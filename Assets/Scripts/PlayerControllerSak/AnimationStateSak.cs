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
    

    bool isAttacking = false; // Variable de estado para el ataque

    // Start is called antes de la primera actualización del frame
    void Start()
    {
        animator = GetComponent<Animator>();
        playerControllerSak = GetComponent<PlayerControllerSak>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isAttackingHash = Animator.StringToHash("isAttacking");
        isJumpingHash = Animator.StringToHash("isJumping");
    }

    // Update se llama una vez por frame
    void Update()
    {
        if (isAttacking) return; // Bloquear el movimiento si el jugador está atacando

        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w") || Input.GetKey("s");
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool runPressed = Input.GetKey("left shift");
        bool attackPressed = Input.GetMouseButton(0);
        bool jumpPressed = Input.GetKeyDown("space");

        bool movementKeyPressed = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

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

        // Si el jugador presiona una tecla de movimiento
        if (!isWalking && movementKeyPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else
        {
            // Si el jugador no está presionando una tecla de movimiento
            if (isWalking && !movementKeyPressed)
            {
                animator.SetBool(isWalkingHash, false);
            }
        }

         // Si el jugador está caminando y presiona la tecla de correr
        if (forwardPressed &&  runPressed)
        {
            animator.SetBool(isRunningHash, true);
        }
        else
        {
            // Si el jugador no está presionando la tecla de correr
            if (!forwardPressed && !runPressed)
            {
                animator.SetBool(isRunningHash, false);
            }
        }

        // Si el jugador presiona la tecla de salto
        if (jumpPressed)
        {
            animator.SetBool(isJumpingHash, true);
            playerControllerSak.Jump();
        }
        else
        {
            // Si el jugador no está presionando la tecla de salto
            if (!jumpPressed)
            {
                animator.SetBool(isJumpingHash, false);
            }
        }
    }
}