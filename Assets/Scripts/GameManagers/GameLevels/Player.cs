using System.Collections.Generic;
using UIWrappers;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRendererWrapper _body;
    [SerializeField] private SpriteRendererWrapper _hair;
    [Space]
    [SerializeField] private List<Color> _colors = new();

    public void Show()
    {
        _body.Show();
        
        _hair.SpriteRenderer.color = Color.clear;
        _hair.Show();
    }
    
    public void ChangeHairColor()
    {
        var idx = Random.Range(0, _colors.Count);
        _hair.SpriteRenderer.color = _colors[idx];
    }

    public void Hide()
    {
        _body.Hide();
        _hair.Hide();
    }
    
    public void SetPosition(Vector3 position) => 
        transform.position = position;
}
