using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eBananaState { NORMAL, DAMAGED, VERY_DAMAGED, DEAD, ORIGINAL }

public class SpaceShipSkin : MonoBehaviour
{
    #region Script Parameters

    [SerializeField]
    private SpriteRenderer currentSprite;

    [Header("Sprites Datas")]

    [SerializeField]
    private Sprite originalSprite;
    [SerializeField]
    private Sprite yellowBanana;
    [SerializeField]
    private Sprite brownBanana;
    [SerializeField]
    private Sprite badBanana;

    #endregion

    #region Fields

    private eBananaState eBananaState;

    #endregion

    public void ChangeBananaState()
    {
        switch (eBananaState)
        {
            case eBananaState.NORMAL:
                eBananaState = eBananaState.DAMAGED;
                break;
            case eBananaState.DAMAGED:
                eBananaState = eBananaState.VERY_DAMAGED;
                break;
            case eBananaState.VERY_DAMAGED:
            case eBananaState.DEAD:
            case eBananaState.ORIGINAL:
                break;
            default:
                break;
        }

        ChangeBananaSprite();
    }

    public void ChangeBananaSprite()
    {
        switch (eBananaState)
        {
            case eBananaState.NORMAL:
                currentSprite.sprite = yellowBanana;
                break;
            case eBananaState.DAMAGED:
                currentSprite.sprite = brownBanana;
                break;
            case eBananaState.VERY_DAMAGED:
                currentSprite.sprite = badBanana;
                break;
            case eBananaState.DEAD:
                break;
            case eBananaState.ORIGINAL:
                currentSprite.sprite = originalSprite;
                break;
            default:
                break;
        }
    }
}
