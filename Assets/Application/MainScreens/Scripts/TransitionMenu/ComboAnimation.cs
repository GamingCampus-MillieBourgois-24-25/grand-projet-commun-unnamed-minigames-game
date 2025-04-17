using UnityEngine;
using TMPro;

public class ComboAnimation : MonoBehaviour
{
    [SerializeField] private  TextMeshProUGUI comboText;
    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 1.4f;
    [SerializeField] private float speed = 2.5f;

    private float _time;
    void Update()
    {
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(_time) + 1) / 2);

        comboText.transform.localScale = new Vector3(scale, scale, 1);

        _time += Time.deltaTime * speed;
    }
}
