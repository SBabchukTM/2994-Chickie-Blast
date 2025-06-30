using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BonusRouletteSpinner
{
    private const float LoopTime = 1;
    private const float AngleStep = 45f;
    private const int LoopsAmount = 4;
    
    public async UniTask Spin(RectTransform roulette, int index)
    {
        UniTaskCompletionSource completion = new UniTaskCompletionSource();
        
        float targetAngle = index * AngleStep;
        float totalRotation = targetAngle + LoopsAmount * 360;
        
        float duration = LoopTime * LoopsAmount;

        roulette
            .DOLocalRotate(new Vector3(0, 0, totalRotation), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuart).OnComplete(() => completion.TrySetResult());
        
        await completion.Task;
    }
}
