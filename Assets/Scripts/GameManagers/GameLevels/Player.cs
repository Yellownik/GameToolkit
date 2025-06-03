using System.Collections.Generic;
using Orbox.Async;
using UIWrappers;
using UnityEngine;
using Utils;

public class Player : BaseUIWrapper
{
    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private SpriteRenderer _hair;
    [Space]
    [SerializeField] private List<Color> _colors = new();
    
    public override IPromise Show()
    {
        _hair.SetAlpha(0);
        
        var promise = BaseScale(true);
        transform.DoMove(true, offset: Vector3.right);
        _body.DoFade(true);

        return promise;
    }

    public override IPromise Hide()
    {
        _hair.DoFade(false);

        var promise = BaseScale(false);
        transform.DoMove(false, offset: Vector3.left);
        _body.DoFade(false);

        return promise;
    }
    
    public void ChangeHairColor()
    {
        var idx = Random.Range(0, _colors.Count);
        _hair.color = _colors[idx];
    }

    public void SetPosition(Vector3 position) => 
        transform.position = position;
}
