using UnityEngine;

public abstract class Foe : MonoBehaviour
{
    public FoeType type;
    public string enemyName;
    public Sprite aliveSprite;
    public Sprite deadSprite;
    public AudioClip attackSound;

    public virtual void Attack() => Debug.Log(enemyName + " attaque !");
    public virtual void Die() => Debug.Log(enemyName + " est KO !");
}
