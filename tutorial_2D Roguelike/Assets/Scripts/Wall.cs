using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public AudioClip chopSound1;
    public AudioClip chopSound2;
    public Sprite damagedSprite;

    private int _hp = 3;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int damage)
    {
        _spriteRenderer.sprite = damagedSprite;

        _hp -= damage;

        if (_hp <= 0)
            gameObject.SetActive(false);
    }
}
