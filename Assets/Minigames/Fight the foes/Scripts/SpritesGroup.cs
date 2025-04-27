using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class SpritesGroup : MonoBehaviour
{

    #region FIELDS
    private Color _color;
    public Color color { get => _color; set => SetColor(value); }
    public bool executeInUpdate;
    public SpriteRenderer[] renderers;

    #endregion

    #region PROPERTIES
    
    #endregion

    #region METHODS

    void GatherSprites()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
#if UNITY_EDITOR

        if (executeInUpdate)
            GatherSprites();
#endif
    }
    #endregion
    #region API

    public void SetColor(Color newColor)
    {
        this._color = newColor;
        for (int i = 0 ; i < renderers.Length; i++)
        {
            var sprite = renderers[i];
            sprite.color = newColor;
        }
    }

    #endregion


} // class