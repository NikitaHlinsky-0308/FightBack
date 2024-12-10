using System;
using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private Transform attackPoint;
        //[SerializeField] private Animator animator;

        private Vector3 punchDirection;
        private PlayerController playerController;
        
        private void Start()
        {
            playerController = GetComponent<PlayerController>();   
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }
        }

        private void Attack()
        {
            playerController.SetPlayerState(PlayerState.Attack);
            
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
            foreach (var enemy in hitEnemies)
            {
                punchDirection = enemy.transform.position - transform.position;
                enemy.GetComponent<Target>().TakeDamage(punchDirection);
            }
            
            playerController.SetPlayerState(PlayerState.Move);
        }
    }
}