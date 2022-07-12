using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteShadow : MonoBehaviour
{
    public Vector2 Offset = new Vector2(-3, -3);
    public Material ShadowMaterial;

    private SpriteRenderer CasterSprite;
    private SpriteRenderer ShadowSprite;
    private Transform CasterTransform;
    private Transform ShadowTransform;

    private void Start()
    {
        CasterTransform = transform;
        ShadowTransform = new GameObject().transform;
        ShadowTransform.parent = CasterTransform;
        ShadowTransform.gameObject.name = "Shadow";
        ShadowTransform.localRotation = Quaternion.identity;

        CasterSprite = GetComponent<SpriteRenderer>();
        ShadowSprite = ShadowTransform.gameObject.AddComponent<SpriteRenderer>();
        ShadowSprite.sprite = CasterSprite.sprite;
        ShadowSprite.material = ShadowMaterial;
        ShadowSprite.sortingLayerName = CasterSprite.sortingLayerName;
        ShadowSprite.sortingOrder = CasterSprite.sortingOrder - 1;
        ShadowSprite.drawMode = CasterSprite.drawMode;
        ShadowSprite.size = CasterSprite.size;
    }

    private void LateUpdate()
    {
        ShadowTransform.position = new Vector2(
            CasterTransform.position.x + Offset.x,
            CasterTransform.position.y + Offset.y);
    }

}

// https://www.youtube.com/watch?v=ft4HUL2bFSQ&ab_channel=GucioDevs