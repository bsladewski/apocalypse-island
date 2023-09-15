using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private void Start()
    {
        InteractionSystem.Instance.OnInteractableSelected += OnInteractableSelected;
        InteractionSystem.Instance.OnInteractableDeselected += OnInteractableDeselected;
    }

    protected abstract void OnInteractableSelected(object sender, GameObject go);

    protected abstract void OnInteractableDeselected(object sender, EventArgs e);
}
