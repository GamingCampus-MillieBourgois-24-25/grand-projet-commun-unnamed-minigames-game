using UnityEngine;

public class TurnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RivalBike"))
        {
            RivalBike rb = other.GetComponent<RivalBike>();
            if (rb != null && rb.ShouldDecideTurn())
            {
                rb.DecideTurnDirection(); // d�clenche la d�cision ici
            }
        }
    }
}
