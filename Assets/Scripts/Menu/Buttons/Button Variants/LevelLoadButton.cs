using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DriftTT
{
    public class LevelLoadButton : BaseButton
    {
        [SerializeField] private int spawnPointId;
        protected override void ButtonBehaviour()
        {
            GameManager.GmInstance.LoadGame(spawnPointId);
        }
    }
}
