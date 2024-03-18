using UnityEngine;
using GameManagement;

    public class ManagingLevelSegments : MonoBehaviour
    {
        [SerializeField] private GameObject[] _levelSegments; // Массив сегментов уровня

        private int _currentSegmentIndex = -1; // Индекс текущего активного сегмента

        private void OnDisable()
        {
            GameManager.Instance.OnGameStarted -= GameStarted;
            GameManager.Instance.OnGameEnded -= GameEnded;
            GameManager.Instance.OnNextSegment -= ActivateNextSegment;
        }

        private void Start()
        {
            GameStarted();
            GameManager.Instance.OnGameStarted += GameStarted;
            GameManager.Instance.OnGameEnded += GameEnded;
            GameManager.Instance.OnNextSegment += ActivateNextSegment;
        }

        private void GameStarted()
        {
            ResetSegments();
            ActivateNextSegment(); // Активируем первый сегмент при старте игры
        }

        private void GameEnded()
        {
            //foreach (var segment in _levelSegments)
            //{
            //    segment.SetActive(false); // Деактивируем все сегменты при завершении игры
            //}
        }

        public void ActivateNextSegment()
        {
            if (_currentSegmentIndex + 1 < _levelSegments.Length)
            {
                _currentSegmentIndex++;
                _levelSegments[_currentSegmentIndex].SetActive(true);
            }
        }

        private void ResetSegments()
        {
            _currentSegmentIndex = -1; // Сбрасываем индекс
            foreach (var segment in _levelSegments)
            {
                segment.SetActive(false); // Деактивируем все сегменты
            }
        }
    }
