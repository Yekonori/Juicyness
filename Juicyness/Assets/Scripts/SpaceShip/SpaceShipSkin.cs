using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eBananaState { NORMAL, DAMAGED, VERY_DAMAGED, DEAD, ORIGINAL }

public class SpaceShipSkin : MonoBehaviour
{
    #region Script Parameters

    [Header("Sprite")]

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

    [Header("Damaged")]

    [SerializeField]
    private float damagedEffectDuration = 0.5f;
    [SerializeField]
    private float damagedEffectGap = 0.1f;

    #endregion

    #region Fields

    private eBananaState eBananaState;

    private float currentDamagedEffect = 0;

    #endregion

    private void Start()
    {
        eBananaState = eBananaState.ORIGINAL;
    }

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
        StartCoroutine(DamagedSprite());
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

    public IEnumerator DamagedSprite()
    {
        while (currentDamagedEffect < damagedEffectGap)
        {
            currentSprite.enabled = !currentSprite.enabled;
            currentDamagedEffect += damagedEffectGap;
            yield return new WaitForSeconds(damagedEffectGap);
        }

        currentDamagedEffect = 0;
        currentSprite.enabled = true;
    }
}
