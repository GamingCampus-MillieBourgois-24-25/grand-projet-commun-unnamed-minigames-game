using UnityEngine;

public class AddCombo : MonoBehaviour
{
    void Awake()
    { 
        ComboManager.Instance.AddCombo(1);
    }
}
