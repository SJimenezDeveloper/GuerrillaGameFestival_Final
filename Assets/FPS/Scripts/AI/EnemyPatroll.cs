using System.Collections;
using UnityEngine;
using UnityEngine.AI;


namespace Unity.FPS.AI
{
    public class EnemyPatrol : MonoBehaviour
    {
        [Tooltip("Ruta de patrulla asignada al enemigo.")]
        public PatrolPath PatrolPath;

        [Tooltip("Velocidad de patrulla del enemigo.")]
        public float patrolSpeed = 3.5f;

        [Tooltip("Tiempo que el enemigo espera en cada nodo de la ruta.")]
        public float waitTimeAtWaypoint = 2f;

        private NavMeshAgent agent;
        private int currentWaypointIndex = 0;
        private bool waiting = false;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError("NavMeshAgent no encontrado en el enemigo.");
                return;
            }

            if (PatrolPath == null || PatrolPath.PathNodes.Count == 0)
            {
                Debug.LogWarning("No se asignó ninguna ruta de patrulla.");
                return;
            }

            agent.speed = patrolSpeed;
            MoveToNextWaypoint();
        }

        void Update()
        {
            if (agent.remainingDistance <= agent.stoppingDistance && !waiting)
            {
                StartCoroutine(WaitAtWaypoint());
            }
        }

        private void MoveToNextWaypoint()
        {
            if (PatrolPath == null || PatrolPath.PathNodes.Count == 0)
                return;

            Vector3 nextPosition = PatrolPath.GetPositionOfPathNode(currentWaypointIndex);
            agent.SetDestination(nextPosition);

            currentWaypointIndex = (currentWaypointIndex + 1) % PatrolPath.PathNodes.Count;
        }

        private IEnumerator WaitAtWaypoint()
        {
            waiting = true;
            yield return new WaitForSeconds(waitTimeAtWaypoint);
            waiting = false;
            MoveToNextWaypoint();
        }
    }
}
