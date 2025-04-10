using UnityEngine;
using UnityEngine.UI;

public class ThreatGauge : MonoBehaviour
{
    public Slider gauge;
    private float fillSpeed;
    private float reduction;

    public void Init(float speed, float reduceAmount)
    {
        fillSpeed = speed;
        reduction = reduceAmount;
        gauge.value = 0;
    }

    void Update()
    {
        gauge.value += fillSpeed * Time.deltaTime;
        if (gauge.value >= 1f)
            Debug.Log("Menace maximale ! Game Over");
    }

    public void Reduce()
    {
        gauge.value = Mathf.Max(0f, gauge.value - reduction);
    }
}
