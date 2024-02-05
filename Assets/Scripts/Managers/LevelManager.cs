using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Game Settings")]
    [Tooltip("Duration of the timer in seconds")]
    [SerializeField] private float timerDuration = 120.0f;
    [SerializeField] private CarControl carControl;
    [SerializeField] private AdManager adManager;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Game UI")]
    [Header("Game Screens")]
    [SerializeField] private GameObject gamePlayUI;
    [SerializeField] private GameObject gameOverUI;
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI timeCounter;
    [SerializeField] private TextMeshProUGUI driftCounter;
    [SerializeField] private TextMeshProUGUI gameOverDriftCounter;

    
    [Space]
    [SerializeField] private Button freeMoneyButton;
    [SerializeField] private Button rewardedMoneyButton;
    [SerializeField] private TextMeshProUGUI freeMoneyText;
    [SerializeField] private TextMeshProUGUI rewardedMoneyText;

    private int _freeMoney;
    private int _rewardedMoney;
    private float _currentTime;
    private float _driftScore;

    
    public static LevelManager LevelManagerInstance { get; private set; }
    
    private void Awake()
    {
        if (LevelManagerInstance != null && LevelManagerInstance != this)
        {
            Destroy(this);
        }
        else
        {
            LevelManagerInstance = this;
        }
    }

    private void Start()
    {
        gameOverUI.SetActive(false);
        gamePlayUI.SetActive(true);
        _currentTime = timerDuration;
        StartCoroutine(UpdateTimer());
    }

    private void Update()
    {
        _driftScore = carControl.DriftScore;
        driftCounter.text = _driftScore.ToString();
    }

    private IEnumerator UpdateTimer()
    {
        while (_currentTime > 0)
        {
            // Update the timer every frame
            yield return null;
            _currentTime -= Time.deltaTime;
            UpdateUIText();
        }

        if (!(_currentTime <= 0)) yield break;
        // Game Over logic here
        GameOver();
        yield return null;
    }

    private void GameOver()
    {
        carControl._generalControls.Gameplay.Disable();
        
        gameOverDriftCounter.text = _driftScore.ToString();
        
        _freeMoney = (int)(_driftScore / 100);
        _rewardedMoney = _freeMoney * 2;
        
        freeMoneyText.text = _freeMoney.ToString();
        rewardedMoneyText.text = _rewardedMoney.ToString();
        
        freeMoneyButton.onClick.AddListener(Payout);
        rewardedMoneyButton.onClick.AddListener(InvokeRewardedAd);
        
        gamePlayUI.SetActive(false);
        gameOverUI.SetActive(true);
    }

    private void Payout()
    {
        GameManager.GmInstance.AddMoney(_freeMoney);
        SceneManager.LoadScene(0);
    }

    private void RewardedPayout()
    {
        GameManager.GmInstance.AddMoney(_rewardedMoney);
        SceneManager.LoadScene(0);
    }
    
    private void InvokeRewardedAd()
    {
        
#if UNITY_EDITOR
        RewardedPayout();
#else
        adManager.ShowRewarded(RewardedPayout);
#endif
        
    }
    
    private void UpdateUIText()
    {
        // Convert seconds to minutes and seconds
        int minutes = Mathf.FloorToInt(_currentTime / 60);
        int seconds = Mathf.FloorToInt(_currentTime % 60);

        // Update the TextMeshProUGUI object with the formatted time
        timeCounter.text = _currentTime <= 0 ? "0" : $"{minutes:00}:{seconds:00}";
    }

    public void Initialize(GameObject carGO, int pointToLoadId)
    {
        var spawnPosTransform = spawnPoints[pointToLoadId];
        var createdCar = Instantiate(carGO, spawnPosTransform.position, spawnPosTransform.rotation);
        _virtualCamera.Follow = createdCar.transform;
        _virtualCamera.LookAt = createdCar.transform;
        carControl = createdCar.GetComponent<CarControl>();
    }
}
