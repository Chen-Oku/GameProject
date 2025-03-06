using UnityEngine;
using TMPro;

namespace FreeflowCombatSpace
{
    public class Health : MonoBehaviour
    {
        public int health = 100;
        public TextMeshProUGUI healthUI;

        Animator anim;
        Vector3 startPos;
        bool isDead;


        void Start()
        {
            healthUI.text = health.ToString();
            anim = GetComponent<Animator>();
            startPos = transform.position;
        }


        public void Hit(int hitPoints)
        {
            health -= hitPoints;
            if (health < 0) health = 0;

            healthUI.text = health.ToString();

            if (health <= 0) {
                Die();
            }
        }


        void Die()
        {
            if (isDead) return;
            
            // play die animation
            anim.SetTrigger("Die");

            // set the enemy as not attackble
            GetComponent<FreeflowCombatEnemy>().isAttackable = false;

            // deactivate rotation script
            GetComponent<LookAtPlayer>().enabled = false;

            isDead = true;
        }

        public void RefreshUI()
        {
            healthUI.text = health.ToString();
            transform.position = startPos;
            isDead = false;
        }
    }
}

