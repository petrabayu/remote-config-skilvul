using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

public class CardPlayer : MonoBehaviour
{
    public Transform atkPosRef;
    public Card chosenCard;
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] public AudioSource attackDamage;
    public TMP_Text healthText;
    public HealthBar healthBar;
    public float Health;
    // public float MaxHealth;
    public PlayerStats stats = new PlayerStats
    {
        MaxHealth = 100,
        RestoreValue = 7,
        DamageValue = 15
    };

    private Tweener animationTweener;
    public TMP_Text NicknameText { get => nicknameText; }

    public bool IsReady = false;

    public void start()
    {
        Health = stats.MaxHealth;
    }

    public void SetStats(PlayerStats newStats, bool restoreFullHealth = false)
    {
        this.stats = newStats;
        if (restoreFullHealth)
            Health = stats.MaxHealth;

        UpdateHealthBar();
    }

    public Attack? AttackValue
    {
        get => chosenCard == null ? null : chosenCard.AttackValue;
        // get
        // {
        //     if (chosenCard == null)
        //         return null;
        //     else
        //         return chosenCard.AttackValue;
        // }
    }

    public void Reset()
    {
        if (chosenCard != null)
        {
            chosenCard.Reset();
        }

        chosenCard = null;
    }

    public void SetChoosenCard(Card newcard)
    {
        if (chosenCard != null)
        {
            chosenCard.transform.DOKill();
            chosenCard.Reset();
        }

        chosenCard = newcard;
        chosenCard.transform.DOScale(chosenCard.transform.localScale * 1.2f, 0.1f);
    }

    public void AnimateAttack()
    {
        animationTweener = chosenCard.transform.DOMove(atkPosRef.position, 1);

    }

    public void AnimateDamage()
    {
        var image = chosenCard.GetComponent<Image>();
        animationTweener = image
        .DOColor(Color.red, 0.1f)
        .SetLoops(3, LoopType.Yoyo)
        .SetDelay(0.2f);
        attackDamage = GetComponent<AudioSource>();
        attackDamage.Play();
    }
    public bool isAnimating()
    {
        return animationTweener.IsActive();
    }

    public void AnimateDraw()
    {
        animationTweener = chosenCard.transform
        .DOMove(chosenCard.originalPosition, 1)
        .SetEase(Ease.InOutQuad)
        .SetDelay(0.1f);
    }



    public void ChangeHealth(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0, stats.MaxHealth);
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        // Health Bar
        healthBar.UpadateBar(Health / stats.MaxHealth);

        healthText.text = Health + " / " + stats.MaxHealth;
    }
    public void isClickable(bool value)
    {
        Card[] cards = GetComponentsInChildren<Card>();
        foreach (var card in cards)
        {
            card.setClickable(value);
        }
    }

}


