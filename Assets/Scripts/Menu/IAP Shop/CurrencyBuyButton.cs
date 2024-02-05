using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace DriftTT
{
    public class CurrencyBuyButton : MonoBehaviour
    {
        private CodelessIAPButton _iapButton;
        private void Start()
        {
            _iapButton = GetComponent<CodelessIAPButton>();
            _iapButton.onPurchaseComplete.AddListener(BuyMoney);
        }

        private void BuyMoney(Product product)
        {
            GameManager.GmInstance.AddMoney((int)product.definition.payout.quantity);
        }
    }
}
