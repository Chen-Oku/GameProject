using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FreeflowCombatSpace;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class FreeflowCombat : MonoBehaviour
{
    [Tooltip("Automatically get game camera on start.")]
    public bool autoGetCamera = true;
    [Tooltip("Set the game camera.")]
    public Transform camera;

    [Tooltip("Set the value of your input (system) X-axis without normalizing. This should be done every frame through a script.")]
    public float xInput;
    [Tooltip("Set the value of the input (system) Y-axis without normalizing. This should be done every frame through a script.")]
    public float yInput;
    
    [Tooltip("The layers of enemies.")]
    public LayerMask enemyLayers;
    [Tooltip("The radius that detects surrounding enemies and can traverse to them.")]
    public float detectionRadius = 10f;
    [Tooltip("If set to true will show the detection radius in scene view as a yellow wire sphere.")]
    public bool showDetectionRadius = true;

    [Tooltip("The time of isTraversing, moving from one enemy to another.")]
    public float traversalTime = 5f;

    [Tooltip("The state name for your idle animation.")]
    public string idleAnimName;
    [Tooltip("The speed of transition from whatever animation currently playing to idle animation.")]
    public float idleAnimTSpeed = 0.3f;

    [Tooltip("Set all your attack animation names and their attack distance.")]
    public AttackAnimations[] attackAnimations;
    [Tooltip("Attack Animations Transition Speed - set the speed of the animation transition.")]
    public float attackAnimsTSpeed = 0.3f;
    [Tooltip("If set to true, on each attack a random attack animation will be chosen and played. if set to false, animations will be played linearly one after another in a looping fashion.")]
    public bool randomizeAttackAnim = false;

    [Tooltip("If set to true, animations will play during traversal, when attack distance is reached then the attack animations will play. If set to false attack animations will during traversal.")]
    public bool useTraversalAnimations = false;
    [Tooltip("Play the traversal animations if the distance between player and enemy is bigger or equal to this.")]
    public float applyTraversalAnimDistance;
    [Tooltip("Set all your traversal animations. Traversal animations are the animations played when you're moving from enemy to another.")]
    public TraversalAnimations[] traversalAnimations;
    [Tooltip("Traversal Animations Transition Speed - set the speed of the animation transition.")]
    public float traversalAnimsTSpeed = 0.3f;
    [Tooltip("If set to true, on each traversal a random traversal animation will be chosen and played. If set to false, animations will be played linearly one after another in a looping fashion.")]
    public bool randomizeTraversalAnim = false;
    [Tooltip("If set to true, the player will traverse to the enemy while maintaining it's current Y position value, if set to false the player will traverse to where the FreeflowCombatEnemy script Y position is.")]
    public bool maintainYPosition = true;

    [Tooltip("Add the scripts you want to disable when attacking and they will be automatically re-enabled. You will most probably have to add your movement script.")]
    public MonoBehaviour[] scriptsToDisable;


    #region SYSTEM VARS

    public FreeflowCombatEnemy currentTarget { get; set; }
    public bool isTraversing { get; private set; }
    public bool isAttacking { get; private set; }

    Animator anim;
    AnimationManager animManager;
    CharacterController controller;

    int attackAnimIndex = -1;
    int traversalAnimIndex = -1;

    float currentHitDist;
    
    bool reachedAttackPosition = false;
    bool attacked = false;
    bool defaultRootMotion;

    FreeflowCombatEnemy nextEnemy;
    
    #endregion

    #region UNITY METHODS

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.updateMode = AnimatorUpdateMode.Normal;
        animManager = new AnimationManager(anim);
        
        controller = GetComponent<CharacterController>();
        defaultRootMotion = anim.applyRootMotion;

        if (autoGetCamera) camera = Camera.main.transform;
    }

    void Update()
    {
        // player reached attack position
        if (reachedAttackPosition) {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99) {
                StopAttacking();
            }
        }

        var inputDirection = (camera.forward * yInput) + (camera.right * xInput);
        inputDirection = inputDirection.normalized;
        inputDirection = new Vector3(inputDirection.x, 0f, inputDirection.z);
    }

    void OnValidate()
    {
        if (GetComponent<Animator>()) {
            GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (showDetectionRadius) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }

    #endregion


    // public method for attacking
    public void Attack()
    {
        xInput = Mathf.Round(xInput);
        yInput = Mathf.Round(yInput);
        
        var inputDirection = (camera.forward * yInput) + (camera.right * xInput);
        inputDirection = inputDirection.normalized;
        inputDirection = new Vector3(inputDirection.x, 0f, inputDirection.z);


        if (attacked) {
            if (inputDirection.x == 0 && inputDirection.z == 0) {
                nextEnemy = currentTarget;
                return;
            }

            Transform tempEnemy = GetTarget(inputDirection);
            if (tempEnemy) {
                nextEnemy = tempEnemy.GetComponent<FreeflowCombatEnemy>();
                return;
            }
        }

        // disable animator root motion
        anim.applyRootMotion = false;


        // check if next target is ready
        if (nextEnemy) {
            currentTarget = nextEnemy;
            nextEnemy = null;
        }

        
        // if no input get enemy in front or from distance
        if (inputDirection.x == 0f && inputDirection.z == 0f) {
            inputDirection = (transform.forward).normalized;

            Transform target = GetTarget(inputDirection);
            SetTarget(target);
        }
        else {
            Transform target = GetTarget(inputDirection);
            SetTarget(target);
        }


        if (!currentTarget) return;


        attacked = true;
        controller.enabled = false;
        DisableScripts();


        // set attack animation index
        if (randomizeAttackAnim) {
            attackAnimIndex = Random.Range(0, attackAnimations.Length);
        }
        else {
            if (attackAnimIndex+1 >= attackAnimations.Length) attackAnimIndex = 0;
            else attackAnimIndex++;
        }
        

        // set traversal animation index
        if (randomizeTraversalAnim) {
            traversalAnimIndex = Random.Range(0, traversalAnimations.Length);
        }
        else {
            if (traversalAnimIndex+1 >= traversalAnimations.Length) traversalAnimIndex = 0;
            else traversalAnimIndex++;
        }
        

        currentHitDist = attackAnimations[attackAnimIndex].attackDistance;
        if (useTraversalAnimations) {
            float dist = (currentTarget.transform.position - transform.position).sqrMagnitude;
            if (dist >= applyTraversalAnimDistance * applyTraversalAnimDistance) {
                animManager.Play(traversalAnimations[traversalAnimIndex].animationName, traversalAnimsTSpeed);
            }
        }


        transform.LookAt(currentTarget.transform);
        StartCoroutine(Lerp());
    }

    // check surrounding for enemies and returns the best one
    Transform GetTarget(Vector3 inputDirection)
    {
        // get surrounding enemies within radius
        Collider[] hitColliders = new Collider[15];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position + controller.center, detectionRadius, hitColliders, enemyLayers);
        List<Transform> targetsList = new List<Transform>();


        // fire a raycast to check no obstacle between player and target
        for (int i = 0; i < numColliders; i++) {
            RaycastHit hitCheck;
            if (Physics.Raycast(transform.position + controller.center, hitColliders[i].transform.position - transform.position, out hitCheck, Mathf.Infinity, Physics.AllLayers))
            {
                if (hitCheck.transform.IsChildOf(hitColliders[i].transform)) {
                    // all valid targets in this list
                    targetsList.Add(hitCheck.transform);
                }
            }
        }


        // check and get the enemy with the best direction
        float bestAngle = -1;
        Transform bestTarget = null;

        for (int i=0; i<targetsList.Count; i++) {
            FreeflowCombatEnemy enemyScript = targetsList[i].GetComponent<FreeflowCombatEnemy>();
            
            // if enemy script isn't present -> run next loop
            if (!enemyScript) {
                continue;
            }

            if (!enemyScript.isAttackable) {
                continue;
            }
            
            Vector3 toOther = (new Vector3(targetsList[i].position.x, transform.position.y, targetsList[i].position.z) - transform.position).normalized;
            float dot = Vector3.Dot(inputDirection, toOther);

            if (dot < 0) {
                continue;
            }

            if (dot > bestAngle) {
                bestAngle = dot;
                bestTarget = targetsList[i];
            }
        }


        return bestTarget;
    }

    // check current transform and set as target if eligible
    void SetTarget(Transform targetTransform)
    {
        if (targetTransform == null) {
            return;
        }

        FreeflowCombatEnemy script = targetTransform.GetComponent<FreeflowCombatEnemy>();

        if (!script) {
            currentTarget = null;
            return;
        }

        if (!script.isAttackable) {
            currentTarget = null;
            return;
        }

        currentTarget = script;
    }

    // lerp to attack position
    IEnumerator Lerp()
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = currentTarget.transform.position;

        while (time < traversalTime) {
            targetPosition = currentTarget.transform.position;
            if (maintainYPosition) targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
            
            float dist = (targetPosition - transform.position).sqrMagnitude;
            bool distCheck = dist <= currentHitDist * currentHitDist;
            
            isTraversing = true;


            if (distCheck) {
                animManager.Play(attackAnimations[attackAnimIndex].animationName, attackAnimsTSpeed);
                isAttacking = true;
                isTraversing = false;

                yield return new WaitForSeconds(attackAnimsTSpeed);

                reachedAttackPosition = true;
                StopAllCoroutines();

                yield return null;
            }
            

            transform.position = Vector3.Lerp(startPosition, targetPosition, time / traversalTime);
            time += Time.deltaTime;

            yield return null;
        }

        transform.position = targetPosition;
    }

    // public method for stopping the attack
    public void StopAttacking()
    {
        StopAllCoroutines();

        reachedAttackPosition = false;
        animManager.Play(idleAnimName, idleAnimTSpeed);
        isAttacking = false;

        AfterAttack();
    }

    // cool down before re-enabling components
    void AfterAttack()
    {
        controller.enabled = true;
        currentTarget = null;

        EnableScripts();
        
        attacked = false;
        anim.applyRootMotion = defaultRootMotion;
        
        if (nextEnemy) Attack();
    }

    // method for reenabling the disabled scripts during attacking
    void EnableScripts()
    {
        foreach (var script in scriptsToDisable) {
            script.enabled = true;
        }
    }

    // method for disabling all set scripts in preparation for attacking
    void DisableScripts()
    {
        foreach (var script in scriptsToDisable) {
            script.enabled = false;
        }
    }
}
