using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniTurtleState : MonoBehaviour
{

    Animator animator;
    int idleHash;
    int walkHash;
    int dieHash;
    int AttackHash;
    int getHitHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        idleHash = Animator.StringToHash("idle");
        walkHash = Animator.StringToHash("walk");
        dieHash = Animator.StringToHash("Die");
        AttackHash = Animator.StringToHash("Attack1");
        getHitHash = Animator.StringToHash("GetHit");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(walkHash);
        bool isIdle = animator.GetBool(idleHash);
        bool isAttacking = animator.GetBool(AttackHash);
        bool isGettingHit = animator.GetBool(getHitHash);

    }
}
