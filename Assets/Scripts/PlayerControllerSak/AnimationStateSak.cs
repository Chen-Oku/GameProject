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

    // Start is called antes de la primera actualizaci�n del frame
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
        if (isAttacking) return; // Bloquear el movimiento si el jugador est� atacando

        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w") || Input.GetKey("s");
        bool horizontalPressed = Input.GetKey("a") || Input.GetKey("d");
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool runPressed = Input.GetKey("left shift");
        bool attackPressed = Input.GetMouseButton(0);
        bool jumpPressed = Input.GetKeyDown("space");

        bool movementKeyPressed = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        if (Input.GetMouseButtonDown(0)) // Bot�n izquierdo del rat�n para atacar
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
            // Si el jugador no est� presionando una tecla de movimiento
            if (isWalking && !movementKeyPressed)
            {
                animator.SetBool(isWalkingHash, false);
            }
        }

        // Si el jugador presiona la tecla de avanzar y también la tecla de correr
        if (forwardPressed || horizontalPressed)
        {
            if (runPressed)
            {
                animator.SetBool(isRunningHash, true);
                animator.SetBool(isWalkingHash, false); // Asegúrate de que no esté caminando
                print("isRunning");
            }
            else
            {
                animator.SetBool(isRunningHash, false);
                animator.SetBool(isWalkingHash, true); // Cambia a caminar si no está corriendo
            }
        }
        else
        {
            // Si no está presionando avanzar, no está caminando ni corriendo
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isWalkingHash, false);
        }

        // Si el jugador presiona la tecla de salto
        if (jumpPressed)
        {
            animator.SetBool(isJumpingHash, true);
            playerControllerSak.Jump();
        }
        else
        {
            // Si el jugador no est� presionando la tecla de salto
            if (!jumpPressed)
            {
                animator.SetBool(isJumpingHash, false);
            }
        }
    }
}