using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour
{
    private Button _myButton;
    
    private void Start()
    {
        _myButton = GetComponent<Button>();
        _myButton.onClick.AddListener(ButtonBehaviour);
    }

    protected abstract void ButtonBehaviour();
}
