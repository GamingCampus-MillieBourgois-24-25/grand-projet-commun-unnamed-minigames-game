using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muffin : Foe
{
    FoeType FoeType = FoeType.Food;

    [SerializeField] Sprite DeadSprite;

    protected override void Die(Action callBack)
    {
        StartCoroutine(PlayDeathAnimation(() => base.Die(callBack)));
        
    }

    IEnumerator PlayDeathAnimation(Action callBack)
    {
        spriteRenderer.sprite = DeadSprite;
        yield return new WaitForSeconds(2);
        callBack.Invoke();
    }
}
