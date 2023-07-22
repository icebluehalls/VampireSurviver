using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private Slider expSlider;
    [SerializeField] private Image[] hpImages;
    [SerializeField] private Image[] _levelUpCards;
    [SerializeField] private GameObject gameOverObject;
    [SerializeField] private TMP_Text duringTimeText;
    [SerializeField] private TMP_Text levelUpText;
    [SerializeField] private TMP_Text killEnemyText;
    [SerializeField] private Button restartButton;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void UpdateKillEnemy(int enemy)
    {
        killEnemyText.text = "Kill : " + enemy;
    }
    public void UpdateHp(int hp)
    {
        for (int i = 0; i < hp; i++)
        {
            hpImages[i].gameObject.SetActive(true);
        }
        for (int i = hp; i < hpImages.Length; i++)
        {
            hpImages[i].gameObject.SetActive(false);
        }
    }

    public void ShowGameOver(DateTime duringTime)
    {
        gameOverObject.SetActive(true);
        
        TimeSpan difference = DateTime.Now - duringTime;

        
        string timeDifferenceFormatted = String.Format("{0:00}:{1:00}:{2:00}", 
            difference.Hours, 
            difference.Minutes, 
            difference.Seconds);

        duringTimeText.text = "during Time : " + timeDifferenceFormatted;
    }

    public void SetLevelUp(int level)
    {
        levelUpText.text = "Level " + level;
    }

    public void Reset()
    {
        gameOverObject.SetActive(false);
        CloseLevelUpCard();
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() => SceneManager.LoadScene("InGameScene"));
    }

    public void UpdateSliderBar(float xp, float maxXp)
    {
        expSlider.value = xp / maxXp;
    }
    
    public void ShowLevelUpCard(LevelUpEvent[] levelUpEvents)
    {
        for (int i = 0; i < levelUpEvents.Length; i++)
        {
            _levelUpCards[i].gameObject.SetActive(true);
            _levelUpCards[i].GetComponentInChildren<TMP_Text>().text = (i + 1).ToString() + "\n" + GameManager.Instance.GetCardInfo(levelUpEvents[i]);
        }
    }

    public void CloseLevelUpCard()
    {
        for (int i = 0; i < _levelUpCards.Length; i++)
        {
            _levelUpCards[i].gameObject.SetActive(false);
        }
    }
}
