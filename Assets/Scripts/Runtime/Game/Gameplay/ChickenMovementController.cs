using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Core.Audio;
using Runtime.Game.Services.Audio;
using UnityEngine;

public class ChickenMovementController
{
    private const float StepTime = 0.15f;
    
    private readonly IAudioService _audioService;
    
    private TilePainter _tilePainter;
    
    public ChickenMovementController(TilePainter tilePainter, IAudioService audioService)
    {
        _tilePainter = tilePainter;
        _audioService = audioService;
    }
    
    public async UniTask MoveChicken(Transform chickenTransform, List<Vector3> travelPoints, bool playAudioOnStep)
    {
        if(travelPoints.Count <= 1)
            return;
        
        UniTaskCompletionSource completion = new UniTaskCompletionSource();
        
        Sequence sequence = DOTween.Sequence();
        
        if (chickenTransform.position.x < travelPoints[1].x)
        {
            var scale = chickenTransform.localScale;
            chickenTransform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
        }
        else
        {
            var scale = chickenTransform.localScale;
            chickenTransform.localScale = new Vector3(- Mathf.Abs(scale.x), scale.y, scale.z);
        }
        
        foreach (var point in travelPoints)
        {
            var capturedPoint = point;
            sequence.Append(
                chickenTransform.DOMove(capturedPoint, StepTime)
                    .SetEase(Ease.Linear)
                    .SetLink(chickenTransform.gameObject)
                    .OnComplete(() =>
                    {
                        if (playAudioOnStep)
                            _audioService.PlaySound(ConstAudio.StepSound);
                        _tilePainter.PaintTile(capturedPoint);
                    })
            );
        }

        sequence.OnComplete(() => completion.TrySetResult());
        sequence.Play();
        
        await completion.Task;
    }
}
