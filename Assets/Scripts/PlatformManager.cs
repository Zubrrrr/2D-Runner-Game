using System.Collections.Generic;
using UnityEngine;
using GameManagement;

public class PlatformManager : MonoBehaviour
{
    private List<IPlatform> _activePlatforms = new List<IPlatform>();

    private void OnDisable()
    {
        GameManager.Instance.OnRegisterPlatform -= RegisterPlatform;
        GameManager.Instance.OnUnregisterPlatform -= UnregisterPlatform;
        GameManager.Instance.OnGameStarted -= StartPlatforms;
        GameManager.Instance.OnGameEnded -= StopPlatforms;
    }

    private void Start()
    {
        GameManager.Instance.OnRegisterPlatform += RegisterPlatform;
        GameManager.Instance.OnUnregisterPlatform += UnregisterPlatform;
        GameManager.Instance.OnGameStarted += StartPlatforms;
        GameManager.Instance.OnGameEnded += StopPlatforms;
    }

    private void StartPlatforms()
    {
        foreach (IPlatform platform in _activePlatforms)
        {
            platform.StartMoving();
        }
    }

    private void StopPlatforms()
    {
        foreach (IPlatform platform in _activePlatforms)
        {
            platform.StopMoving();
        }
    }

    public void RegisterPlatform(IPlatform platform)
    {
        if (!_activePlatforms.Contains(platform))
        {
            _activePlatforms.Add(platform);
             platform.StartMoving();
        }
    }

    public void UnregisterPlatform(IPlatform platform)
    {
        if (_activePlatforms.Contains(platform))
        {
            _activePlatforms.Remove(platform);
        }
    }
}
