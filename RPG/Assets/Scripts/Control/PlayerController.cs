﻿using UnityEngine;
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
            if (InteractWithCombat())
                return;
            if (InteractWithMovement())
                return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.gameObject.GetComponent<CombatTarget>();
                if (target == null)
                    continue;

                if (Input.GetMouseButtonDown(0))
                    GetComponent<Fighter>().Attack(target);

                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {

            Ray ray = GetMouseRay();
            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f);

            RaycastHit hit;
            if (Physics.Raycast(GetMouseRay(), out hit))
            {
                if (Input.GetMouseButton(0))
                    _mover.StartMoveAction(hit.point);

                return true;
            }

            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
