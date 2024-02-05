using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DriftTT
{
    public class CarBuyButton : BaseButton
    {
        [SerializeField] private GameObject priceText;
        [SerializeField] private Image carPreviewImage;

        private int _carToBuyId;
        
        private Action<int> _carChooseAction;
        public void InitOnSpawn(int carId, int carStatus, Action<int> carChoose, Sprite carPreview)
        {
            _carChooseAction = carChoose;
            _carToBuyId = carId;
            carPreviewImage.sprite = carPreview;
            if (carStatus == 1)
            {
                priceText.SetActive(false);
            }
        }

        public void RemoveCarPrice()
        {
            priceText.SetActive(false);
        }

        protected override void ButtonBehaviour()
        {
            _carChooseAction(_carToBuyId);
        }
    }
}
