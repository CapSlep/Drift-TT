using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DriftTT
{
    public class CurrencyDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;

        private void OnEnable()
        {
            moneyText.text = GameManager.GmInstance.GetMoney().ToString();
            GameManager.GmInstance.MoneyUpdate += RefreshMoneyText;
        }

        private void OnDisable()
        {
            GameManager.GmInstance.MoneyUpdate -= RefreshMoneyText;
        }

        private void RefreshMoneyText(int value)
        {
            moneyText.text = value.ToString();
        }
    }
}
