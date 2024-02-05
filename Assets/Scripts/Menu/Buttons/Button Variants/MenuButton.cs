using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : BaseButton
{
    [SerializeField] private GameObject screenToOpen;

    protected override void ButtonBehaviour()
    {
        UIController.UIInstance.OpenScreen(screenToOpen);
    }

}
