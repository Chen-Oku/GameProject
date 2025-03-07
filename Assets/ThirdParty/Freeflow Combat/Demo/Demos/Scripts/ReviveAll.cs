using UnityEngine;

namespace FreeflowCombatSpace
{
    public class ReviveAll : MonoBehaviour
    {
        public GameObject[] enemies;



        void Start()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] == null)
                {
                    Debug.LogWarning($"Enemy at index {i} is null at start.");
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R)) {
                ReviveEnemies();
            }
        }

        void ReviveEnemies()
        {
            int max = enemies.Length;

            for (int i = 0; i < max; i++)
            {
                if (enemies[i] == null)
                {
                    Debug.LogWarning($"Enemy at index {i} is null.");
                    continue;
                }

                Health health = enemies[i].GetComponent<Health>();
                Animator anim = enemies[i].GetComponent<Animator>();
                LookAtPlayer lap = enemies[i].GetComponent<LookAtPlayer>();
                FreeflowCombatEnemy freeFlowEnemy = enemies[i].GetComponent<FreeflowCombatEnemy>();

                if (health == null || anim == null || lap == null || freeFlowEnemy == null)
                {
                    Debug.LogWarning($"Missing component(s) on enemy at index {i}.");
                    continue;
                }

                if (health.health <= 0)
                {
                    Debug.Log($"Reviving enemy at index {i}.");
                    anim.SetTrigger("Revive");
                    health.health = 50;
                    health.RefreshUI();
                    lap.enabled = true;
                    freeFlowEnemy.isAttackable = true;
                }
            }
        }

        //void ReviveEnemies()
        //{
        //    int max = enemies.Length;

        //    for (int i=0; i<max; i++) {
        //        Health health = enemies[i].GetComponent<Health>();
        //        Animator anim = enemies[i].GetComponent<Animator>();
        //        LookAtPlayer lap = enemies[i].GetComponent<LookAtPlayer>();
        //        FreeflowCombatEnemy freeFlowEnemy = enemies[i].GetComponent<FreeflowCombatEnemy>();

        //        if (health.health <= 0) {
        //            anim.SetTrigger("Revive");
        //            health.health = 50;
        //            health.RefreshUI();
        //            lap.enabled = true;
        //            freeFlowEnemy.isAttackable = true;
        //        }
        //    }
        //}
    }
}
