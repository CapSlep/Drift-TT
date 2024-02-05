using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnButton : BaseButton
{
    protected override void ButtonBehaviour()
    {
        UIController.UIInstance.ReturnToMenu();
    }
}
