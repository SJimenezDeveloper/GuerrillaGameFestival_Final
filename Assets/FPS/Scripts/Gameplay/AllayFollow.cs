using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Game;
using UnityEngine.AI;

namespace Unity.FPS.Gameplay
{
    public class AllayFollow : MonoBehaviour
    {
        public NavMeshAgent Agent;
        public Animator Anim;
        public float FollowDistance = 5f;
        public float StopDistance = 2f;
        private PlayerCharacterController TargetToFollow; // Personaje a seguir
        private PlayerCharacterController CurrentController; // Referencia propia

        void Start()
        {
            CurrentController = GetComponent<PlayerCharacterController>();
            UpdateFollowTarget();
        }

        void Update()
        {
            if (CurrentController.SoldierState == SoldierState.Autonomous && TargetToFollow != null)
            {
                FollowControlledPlayer();
            }
        }

        void FollowControlledPlayer()
        {
            float distance = Vector3.Distance(transform.position, TargetToFollow.transform.position);

            // Moverse hacia el objetivo si está a más de una distancia mínima
            if (distance > StopDistance && distance <= FollowDistance)
            {
                Agent.SetDestination(TargetToFollow.transform.position);
                if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("jog")) // Evita activar repetidamente la animación
                {
                    Anim.SetTrigger("jog");
                }
            }
            else if (distance <= StopDistance)
            {
                Agent.ResetPath();

                if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("idle")) // Evita activar repetidamente la animación
                {
                    Anim.SetTrigger("idle");
                }
            }
        }

        public void UpdateFollowTarget()
        {
            // Buscar al personaje que está en estado Controlled
            foreach (PlayerCharacterController player in FindObjectsOfType<PlayerCharacterController>())
            {
                if (player.SoldierState == SoldierState.Controlled)
                {
                    TargetToFollow = player;
                    break;
                }
            }
        }
    }
}

