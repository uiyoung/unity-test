using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        private Transform target;
        [SerializeField] float weaponRage = 1.5f;

        private void Update()
        {
            if (target == null)
                return;

            if (!IsInRange()) 
                GetComponent<Mover>().MoveTo(target.position);
            else
                GetComponent<Mover>().Cancel();
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.position) <= weaponRage;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }
    }
}
