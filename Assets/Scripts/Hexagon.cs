using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private new Renderer renderer;
    [SerializeField] Collider collider;

    public HexaStack HexStack { get; private set; }
    public Color Color
    {
        get => renderer.material.color;
        set => renderer.material.color = value; 
    }

    //Set hexa hiện tại cho 1 cái stack chứa nó
    public void Configure(HexaStack hexStack)
    {
        HexStack = hexStack;
    }

    public void DisableCollider()
    {
        collider.enabled = false;
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void MoveToLocal(Vector3 targetPos)
    {
        LeanTween.cancel(gameObject);
        transform.DOKill();
        float delay = transform.GetSiblingIndex() * 0.01f;
        transform.DOLocalMove(targetPos, 0.2f)
            .SetEase(Ease.Linear)
            .SetDelay(delay);

        Vector3 dir = (targetPos - transform.localPosition).With(y: 0).normalized;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, dir);

        LeanTween.rotateAround(gameObject, rotationAxis, 180, 0.2f)
            .setEase(LeanTweenType.easeInOutSine).setDelay(delay);
        //transform.DOLocalRotate(new Vector3(180, 0, 0), 0.2f)
        //    .SetEase(Ease.InOutSine).SetDelay(delay);
    }

    public void Vanish(float delay)
    {
        transform.DOKill();
        transform.DOScale(Vector3.zero, 0.2f)
            .SetDelay(delay)
            .SetEase(Ease.InOutSine).OnComplete(() => Destroy(gameObject));
    }
}
