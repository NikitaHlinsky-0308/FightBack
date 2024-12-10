using CodeBase.Data;
using CodeBase.Player;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public enum EnemyState
    {
        Patrolling,
        Chasing,
        Attacking
    }

    public class EnemyBrain : MonoBehaviour
    {
        //[Header("Enemy settings")]

        [Header("Patrolling")] [SerializeField]
        private float patrolRadius = 10f;
        //private int _currentPatrolIndex;
        //public Transform[] patrolPoints;

        [Header("Chasing")] [SerializeField] private float _chaseDistance = 10f;
        [SerializeField] private float _offset = 1f;

        [Header("Attacking")] [SerializeField] private float _attackDistance = 5f;


        private NavMeshAgent _navMeshAgent;
        private EnemyState _currentState;
        private Transform target;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _currentState = EnemyState.Patrolling;
            //_currentPatrolIndex = 0;
            target = FindObjectOfType<PlayerController>().transform;
            //_navMeshAgent.SetDestination(patrolPoints[_currentPatrolIndex].position);
        }

        private void Update()
        {
            switch (_currentState)
            {
                case EnemyState.Patrolling:
                    Patrol();
                    break;
                case EnemyState.Chasing:
                    Chase();
                    break;
                case EnemyState.Attacking:
                    Attack();
                    break;
            }

            HandleStateTransitions();
        }

        private void Patrol()
        {
            // patroling with points
            /*if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
            {
                _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
                _navMeshAgent.SetDestination(patrolPoints[_currentPatrolIndex].position);
            }*/

            // prtroling in radius
            
            /*if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * patrolRadius;
                randomDirection += transform.position;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
                {
                    Vector3 finalPosition = hit.position;
                    _navMeshAgent.SetDestination(finalPosition);
                }
            }*/
        }

        private void Chase()
        {
            _navMeshAgent.SetDestination(target.position);
        }

        private void Attack()
        {
            _navMeshAgent.SetDestination(transform.position);
            LookAtTarget();
            // Implement attack logic here, e.g., reduce target's health
            Debug.Log("Attacking...");
        }


        

        private void LookAtTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        private void HandleStateTransitions()
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            switch (_currentState)
            {
                case EnemyState.Patrolling:
                    if (distanceToTarget < _chaseDistance)
                    {
                        _currentState = EnemyState.Chasing;
                    }

                    break;
                case EnemyState.Chasing:
                    if (distanceToTarget < _attackDistance)
                    {
                        _currentState = EnemyState.Attacking;
                    }
                    else if (distanceToTarget > _chaseDistance)
                    {
                        _currentState = EnemyState.Patrolling;
                    }

                    break;
                case EnemyState.Attacking:
                    if (distanceToTarget > _attackDistance)
                    {
                        _currentState = EnemyState.Chasing;
                    }

                    break;
            }
        }

        // make gizmos for chase distance and attack distance
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackDistance);
        }
    }
}