using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

namespace Completed
{
    public class Loader : MonoBehaviour
    {
        [FormerlySerializedAs("gameManager")] public GameObject _gameManager; //GameManager prefab to instantiate.
        [FormerlySerializedAs("soundManager")] public GameObject _soundManager; //SoundManager prefab to instantiate.


        private void Awake()
        {
            //Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
            if (GameManager.Instance == null)

                //Instantiate gameManager prefab
                Instantiate(_gameManager);

            //Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
            if (SoundManager.Instance == null)

                //Instantiate SoundManager prefab
                Instantiate(_soundManager);
        }
    }
}