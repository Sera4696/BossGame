using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rhythm : MonoBehaviour
{
    private float beforScale,afterScale;
    // Start is called before the first frame update
    void Start()
    {
        beforScale = transform.localScale.x;
        afterScale = transform.localScale.x + 1.5f;

    }

    // Update is called once per frame
    void Update()
    {
        //transform.DOScale(new Vector3(28, 28, 28), 1f).SetEase(Ease.InOutCubic).OnComplete(() =>
        //   {
        //       transform.DOScale(new Vector3(26.5f, 26.5f, 26.5f), 1f).SetEase(Ease.InOutCubic);
        //   }).SetDelay(2f);

        if (transform.localScale == new Vector3(afterScale,afterScale,afterScale))
        {
            transform.DOScale(new Vector3(beforScale, beforScale, beforScale), 0.6f).SetEase(Ease.InSine);

        }

        if(transform.localScale == new Vector3(beforScale, beforScale, beforScale))
        {
            transform.DOScale(new Vector3(afterScale, afterScale, afterScale), 0.1f).SetEase(Ease.InOutQuint);
        }
    }
}
