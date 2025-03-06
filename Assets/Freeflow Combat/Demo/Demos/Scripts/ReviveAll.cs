using UnityEngine;

namespace FreeflowCombatSpace
{
    public class ReviveAll : MonoBehaviour
    {
        public GameObject[] enemies;

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

            for (int i=0; i<max; i++) {
                Health health = enemies[i].GetComponent<Health>();
                Animator anim = enemies[i].GetComponent<Animator>();
                LookAtPlayer lap = enemies[i].GetComponent<LookAtPlayer>();
                FreeflowCombatEnemy freeFlowEnemy = enemies[i].GetComponent<FreeflowCombatEnemy>();
                
                if (health.health <= 0) {
                    anim.SetTrigger("Revive");
                    health.health = 50;
                    health.RefreshUI();
                    lap.enabled = true;
                    freeFlowEnemy.isAttackable = true;
                }
            }
        }
    }
}
