using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DriftTT
{
    public class CarShop : MonoBehaviour
    {
        [SerializeField] private Transform carPosition;
        [SerializeField] private GameObject activeCar;
        [SerializeField] private GameObject[] carCatalog;
        [SerializeField] private Sprite[] carsPreviews;

        [SerializeField] private RectTransform carButtonsParent;
        [SerializeField] private CarBuyButton carBuyButtonPrefab;
        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI buyButtonText;

        private const string ActiveCarKey = "ActiveCar";
        private const string CarStatusKey = "CarStatus";
        
        private const string BuyText = "Buy";
        private const string ChooseText = "Choose";

        private int _activeCarId = 0;
        private List<CarItem> _carItems = new List<CarItem>();

        private struct CarItem
        {
            public int CarId;
            public int CarStatus;
            public GameObject CreatedGameObject;
            public CarBuyButton CreatedBuyButton;
            
        }
        
        public static CarShop CarShopInstance { get; private set; }

        private void Awake()
        {
            if (CarShopInstance != null && CarShopInstance != this)
            {
                Destroy(this);
            }
            else
            {
                CarShopInstance = this;
            }
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            PlayerPrefs.SetInt(CarStatusKey + 0, 1);
            for (int i = 0; i < carCatalog.Length; i++)
            {
                var createdCarItem = new CarItem();
                
                var createdBuyButton = Instantiate(carBuyButtonPrefab, carButtonsParent);
                createdCarItem.CreatedBuyButton = createdBuyButton;
                
                var carStatus = GetCarStatus(i);
                createdCarItem.CarStatus = carStatus;
                
                var createdCar = Instantiate(carCatalog[i], carPosition.position, carPosition.rotation, carPosition);
                createdCarItem.CreatedGameObject = createdCar;
                createdCar.GetComponent<CarControl>().ShopExposed();
                createdCar.SetActive(false);
                createdBuyButton.InitOnSpawn(i, carStatus, ChooseCar, carsPreviews[i]);
                
                createdCarItem.CarId = i;
                _carItems.Add(createdCarItem);
            }

            _activeCarId = GetActiveCarId();
            activeCar = _carItems[_activeCarId].CreatedGameObject;
            activeCar.SetActive(true);
        }

        public GameObject GetActiveCar()
        {
            return carCatalog[GetActiveCarId()];
        }

        private int GetActiveCarId()
        {
            if (!PlayerPrefs.HasKey(ActiveCarKey))
            {
                PlayerPrefs.SetInt(ActiveCarKey, 0);
            }

            return _activeCarId = PlayerPrefs.GetInt(ActiveCarKey);
        }

        private int GetCarStatus(int carId)
        {
            return PlayerPrefs.GetInt(CarStatusKey + carId);
        }

        private void ChooseCar(int carId)
        {
            ChangeActiveCar(carId);
            UpdateCarStatusUI(carId);
        }

        private void ChangeActiveCar(int carId)
        {
            activeCar.SetActive(false);
            activeCar = _carItems[carId].CreatedGameObject;
            activeCar.SetActive(true);
        }

        private void UpdateCarStatusUI(int carId)
        {
            var carStatus = GetCarStatus(carId);
            buyButton.onClick.RemoveAllListeners();
            if (carStatus == 0)
            {
                buyButtonText.text = BuyText;
                buyButton.onClick.AddListener(() => BuyCar(carId));
            }
            else
            {
                buyButtonText.text = ChooseText;
                buyButton.onClick.AddListener(() => SetCarAsActive(carId));
            }
        }

        private void BuyCar(int carId)
        {
            if (GameManager.GmInstance.RemoveMoney(10000))
            {
                PlayerPrefs.SetInt(CarStatusKey + carId, 1);
                PlayerPrefs.SetInt(ActiveCarKey, carId);
                UpdateCarStatusUI(carId);
                _carItems[carId].CreatedBuyButton.RemoveCarPrice();
            }
        }

        private void SetCarAsActive(int carId)
        {
            PlayerPrefs.SetInt(ActiveCarKey, carId);
        }

        public void ReturnToMenu()
        {
            ChangeActiveCar(GetActiveCarId());
        }
    }
}