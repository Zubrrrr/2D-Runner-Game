using UnityEngine;
using System;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action OnGameStarted;
        public event Action OnNextSegment;
        public event Action OnGameEnded;
        public event Action OnPlayerJumped; 
        public event Action OnPlayerLanded;

        public event Action<IPlatform> OnRegisterPlatform;
        public event Action<IPlatform> OnUnregisterPlatform;

        public event Action<GameObject> OnEchoEffectCreated;
        public event Action<GameObject> OnEchoEffectDestroyed;

        public event Action<float, float> OnLightIntensityChangeRequested;

        public void StartGame() => OnGameStarted?.Invoke();

        public void EndGame() => OnGameEnded?.Invoke();

        public void ActivateNextSegment() => OnNextSegment?.Invoke();

        public void PlayerJumped() => OnPlayerJumped?.Invoke();

        public void PlayerLanded() => OnPlayerLanded?.Invoke();

        public void RegisterPlatform(IPlatform platform) => OnRegisterPlatform?.Invoke(platform);

        public void UnregisterPlatform(IPlatform platform) => OnUnregisterPlatform?.Invoke(platform);

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
