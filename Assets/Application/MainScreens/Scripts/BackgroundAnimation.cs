using UnityEngine;
using UnityEngine.UI;

public class BackgroundAnimation : MonoBehaviour
{
    public Image background;
    public float minScale = 1f;
    public float maxScale = 1.4f;
    public float speed = 2.5f;

    private float _time;
    void Update()
    {
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(_time) + 1) / 2);

        background.transform.localScale = new Vector3(scale, scale, 1);

        _time += Time.deltaTime * speed;
    }
}
