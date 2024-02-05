using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DriftTT
{
    public class SceneLoader : MonoBehaviour
    {
        private int _startLocationId = 0;
        
        public void LoadMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void LoadGame(int spawnPointId)
        {
            _startLocationId = spawnPointId;
            SceneManager.LoadScene(1);
        }

        public int GetStartLocation()
        {
            return _startLocationId;
        }
    }
}
