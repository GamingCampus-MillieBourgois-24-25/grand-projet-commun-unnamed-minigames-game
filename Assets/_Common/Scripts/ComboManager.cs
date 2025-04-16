using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance {get; private set;}

    [SerializeField] private int combo = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    public void AddCombo(int amount)
    {
        combo += amount;
    }

    public int GetCombo()
    {
        return combo;
    }

    public void ResetCombo()
    {
        combo = 0;
    }
}
