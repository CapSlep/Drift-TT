using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DriftTT;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(SceneLoader))]
public class GameManager : MonoBehaviour
{
    private int _playerMoney = 0;

    private const string MoneyKey = "money";

    private GameObject _activeCar;

    private SceneLoader _sceneLoader;

    public Action<int> MoneyUpdate;

    public static GameManager GmInstance { get; private set; }

    private void Awake()
    {
        if (GmInstance != null && GmInstance != this)
        {
            Destroy(this);
        }
        else
        {
            GmInstance = this;
            DontDestroyOnLoad(this);
        }

        _playerMoney = RefreshMoney();
        _sceneLoader = GetComponent<SceneLoader>();
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += CreatePlayersCar;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= CreatePlayersCar;
    }
    
    #region CurrencyMethods

    public void AddMoney(int amount)
    {
        _playerMoney = RefreshMoney();
        _playerMoney += amount;
        PlayerPrefs.SetInt(MoneyKey, _playerMoney);
        MoneyUpdate?.Invoke(_playerMoney);
    }

    public bool RemoveMoney(int amount)
    {
        _playerMoney = RefreshMoney();
        if (_playerMoney >= amount)
        {
            _playerMoney -= amount;
            PlayerPrefs.SetInt(MoneyKey, _playerMoney);
            MoneyUpdate?.Invoke(_playerMoney);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetMoney()
    {
        _playerMoney = RefreshMoney();
        return _playerMoney;
    }

    private int RefreshMoney()
    {
        return PlayerPrefs.GetInt(MoneyKey);
    }

    #endregion

    #region Level Load Methods

    public void LoadMenu()
    {
        _sceneLoader.LoadMenu();
    }

    public void LoadGame(int spawnPointId)
    {
        _activeCar = CarShop.CarShopInstance.GetActiveCar();
        _sceneLoader.LoadGame(spawnPointId);
    }

    private void CreatePlayersCar(Scene previousScene, Scene currentScene)
    {
        if (previousScene != currentScene && currentScene.buildIndex == 1)
        {
            LevelManager.LevelManagerInstance.Initialize(_activeCar,_sceneLoader.GetStartLocation());
        }
    }

    public int GetStartLocation()
    {
        return _sceneLoader.GetStartLocation();
    }

    #endregion

    #region Car Choose Methods


    #endregion
}