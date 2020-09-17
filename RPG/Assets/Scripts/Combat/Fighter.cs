using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
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
                GetComponent<Mover>().Stop();
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.position) <= weaponRage;
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;

            print("Take that you short, squat peasant!" + target.name);
        }

        public void Cancel()
        {
            target = null;
        }
    }
}
