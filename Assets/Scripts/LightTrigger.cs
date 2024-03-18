using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class LightTrigger : MonoBehaviour
{
    [SerializeField] private float _targetIntensity;
    [SerializeField] private float _duration;
    [SerializeField] private Light2D _light2D;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ChangeIntensityCoroutine(_targetIntensity, _duration));
        }
    }

    private IEnumerator ChangeIntensityCoroutine(float targetIntensity, float duration)
    {
        float timeElapsed = 0;
        float startIntensity = _light2D.intensity;

        while (timeElapsed < duration)
        {
            _light2D.intensity = Mathf.Lerp(startIntensity, targetIntensity, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _light2D.intensity = targetIntensity;
    }
}
