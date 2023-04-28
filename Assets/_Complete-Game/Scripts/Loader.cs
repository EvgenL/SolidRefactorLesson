using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

namespace Completed
{
    public class Loader : MonoBehaviour
    {
        [FormerlySerializedAs("gameManager")] public GameObject _gameManager; //GameManager prefab to instantiate.
        [FormerlySerializedAs("soundManager")] public GameObject _soundManager; //SoundManager prefab to instantiate.
        [SerializeField] private Player _player;

        private void Awake()
        {
            if (GameManager.Instance == null)
                Instantiate(_gameManager);

            if (SoundManager.Instance == null)
                Instantiate(_soundManager);

#if UNITY_STANDALONE || UNITY_WEBPLAYER
            _player.SetInput(new KeyboardInput());
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            _player.SetInput(new TouchInput());
#endif
        }
    }
}