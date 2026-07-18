using System;
using UnityEngine;

public interface IMoneyPileView
{
    void UpdateVisual(int count);
    void PlayCollectEffect(Transform target, Action onArrived);
}
