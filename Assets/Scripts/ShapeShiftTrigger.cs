using System.Collections.Generic;
using UnityEngine;

public enum PlayerForm
{
    Square,
    Airplane
}

[System.Serializable]
public class FormPrefabPair
{
    public PlayerForm form;
    public GameObject prefab;
}

public class ShapeShiftTrigger : MonoBehaviour
{
    [SerializeField] private List<FormPrefabPair> _formPrefabsPairs;
    [SerializeField] private PlayerForm _targetForm;

    private Dictionary<PlayerForm, GameObject> _formMap = new Dictionary<PlayerForm, GameObject>();

    private void Awake()
    {
        foreach (var pair in _formPrefabsPairs)
        {
            _formMap.Add(pair.form, pair.prefab);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            ActivateForm(_targetForm);
        }
    }

    public void ActivateForm(PlayerForm form)
    {
        foreach (var prefab in _formMap.Values)
        {
            prefab.SetActive(false);
        }

        if (_formMap.TryGetValue(form, out GameObject targetPrefab))
        {
            targetPrefab.SetActive(true);
        }
    }
}
