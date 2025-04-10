using System;
using System.Collections;
using System.Collections.Generic;
using Axoloop.Global;
using UnityEngine;

public class Axo : SingletonMB<Axo>
{
    [SerializeField] protected Sprite AliveSprite;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = AliveSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(Action callBack)
    {
        callBack?.Invoke();
    }

    public void Die(Action callBack)
    {
        callBack?.Invoke();
    }
}
