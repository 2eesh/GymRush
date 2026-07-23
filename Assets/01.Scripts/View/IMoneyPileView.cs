using System;
using UnityEngine;

public interface IMoneyPileView
{
    void UpdateVisual(int count);
    void Show();
    void Hide();
    void PlayCollectEffect(Transform target, Action onArrived);
}
