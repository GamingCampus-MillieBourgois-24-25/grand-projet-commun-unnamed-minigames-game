using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Foe
{
    FoeType FoeType = FoeType.Liquid;

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
