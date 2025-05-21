using System;
using UnityEngine;

public class GameTriggerTag : GameTrigger
{
    [SerializeField] string tagToCheck = "Untagged";
    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tagToCheck))
            return;

        base.OnTriggerEnter(other);
    }
}
