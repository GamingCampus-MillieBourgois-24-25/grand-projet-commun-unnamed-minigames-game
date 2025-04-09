using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : Foe
{
    FoeType FoeType = FoeType.Wind;

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
