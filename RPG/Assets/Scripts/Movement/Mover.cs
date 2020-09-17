using RPG.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        private NavMeshAgent _nma;
        private Animator _anim;

        void Start()
        {
            _nma = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
        }

        void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<Fighter>().Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            _nma.isStopped = false;
            _nma.destination = destination;
        }

        public void Stop()
        {
            _nma.isStopped = true;
        }

        private void UpdateAnimator()
        {
            // 1. Get the global velocity from NavMeshAgent
            Vector3 velocity = _nma.velocity;
            // 2. Convert this into a local value relative to the character
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            // 3. Set the Animator's blend value to be equal to our desired forward speed(on the Z axis)
            _anim.SetFloat("forwardSpeed", localVelocity.z);
        }
    } 
}
