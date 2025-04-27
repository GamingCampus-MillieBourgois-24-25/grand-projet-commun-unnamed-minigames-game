using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ScrollingTexture : MonoBehaviour
{
    [SerializeField] Vector2 scroll = Vector2.right;
    [SerializeField] float scrollSpeed = 1f;

    private Image image;
    private Material materialInstance;

#if UNITY_EDITOR
    private void OnEnable()
    {
        EditorApplication.update += EditorUpdate;
    }

    private void OnDisable()
    {
        EditorApplication.update -= EditorUpdate;
    }
#endif

    private void Start()
    {
        SetupMaterial();
    }

    private void SetupMaterial()
    {
        image = GetComponent<Image>();
        if (image != null && image.material != null)
        {
            materialInstance = Instantiate(image.material);
            image.material = materialInstance;
        }
    }

#if UNITY_EDITOR
    private void EditorUpdate()
    {
        if (!Application.isPlaying)
        {
            ScrollTexture();
        }
    }
#endif

    private void Update()
    {
        if (Application.isPlaying)
        {
            ScrollTexture();
        }
    }

    private void ScrollTexture()
    {
        if (materialInstance != null)
        {
            Vector2 offset = materialInstance.mainTextureOffset;
            offset += scroll * scrollSpeed * Time.deltaTime;
            materialInstance.mainTextureOffset = offset;
        }
    }
}
