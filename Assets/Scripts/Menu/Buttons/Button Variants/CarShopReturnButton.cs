using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DriftTT
{
    public class CarShopReturnButton : BaseButton
    {
        [SerializeField] private CarShop _carShop;
        
        protected override void ButtonBehaviour()
        {
            _carShop.ReturnToMenu();
            UIController.UIInstance.ReturnToMenu();
        }
    }
}
