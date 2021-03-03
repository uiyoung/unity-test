using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Breakable"))
            collision.GetComponent<Pot>().Smash();

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = collision.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                Vector2 difference = collision.transform.position - transform.position;
                hit.AddForce(difference.normalized * thrust, ForceMode2D.Impulse);

                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.GetComponent<PlayerMovement>().currentState = PlayerState.stagger;
                    hit.GetComponent<PlayerMovement>().KnockBack(knockTime);
                }

                if (collision.gameObject.CompareTag("Enemy"))
                {
                    collision.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    hit.GetComponent<Enemy>().KnockBack(hit, knockTime, damage);
                }
            }
        }
    }
}
