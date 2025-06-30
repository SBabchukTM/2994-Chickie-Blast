using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    public void SetSkin(Sprite sprite) => _spriteRenderer.sprite = sprite;
}
