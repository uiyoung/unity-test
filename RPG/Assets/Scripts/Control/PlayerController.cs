using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover _mover;

        void Start()
        {
            _mover = GetComponent<Mover>();
        }

        void Update()
        {
            InteractWithCombat();
            InteractWithMovement();
        }

        private void InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.gameObject.GetComponent<CombatTarget>();
                if (target == null)
                    continue;

                if(Input.GetMouseButtonDown(0))
                    GetComponent<Fighter>().Attack(target);
            }
        }

        private void InteractWithMovement()
        {
            if (Input.GetMouseButton(0))
                MoveToCursor();
        }

        private void MoveToCursor()
        {
            Ray ray = GetMouseRay();
            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f);

            RaycastHit hit;
            if (Physics.Raycast(GetMouseRay(), out hit))
            {
                _mover.MoveTo(hit.point);
            }
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
