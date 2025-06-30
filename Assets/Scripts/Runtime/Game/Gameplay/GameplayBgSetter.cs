using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayBgSetter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _bgRenderer;
    [SerializeField] private SpriteRenderer _mapRenderer;

    public void SetVisuals(SectionVisuals bgSectionVisuals)
    {
        _bgRenderer.sprite = bgSectionVisuals.SectionBG;
        _mapRenderer.sprite = bgSectionVisuals.SectionMap;
    }

    public void Enable(bool enable)
    {
        _bgRenderer.enabled = enable;
        _mapRenderer.enabled = enable;
    }
}
