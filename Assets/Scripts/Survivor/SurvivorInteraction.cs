using System;
using UnityEngine;

public class SurvivorInteraction : Interactable
{
    [SerializeField]
    private Outline outline;

    private bool isSelected = false;

    protected override void OnInteractableSelected(object sender, GameObject go)
    {
        if (!isSelected && go.Equals(transform.gameObject))
        {
            SetIsSelected(true);
        }
        else if (isSelected)
        {
            SetIsSelected(false);
        }
    }

    protected override void OnInteractableDeselected(object sender, EventArgs e)
    {
        if (isSelected)
        {
            SetIsSelected(false);
        }
    }

    private void SetIsSelected(bool isSelected)
    {
        outline.enabled = isSelected;
        this.isSelected = isSelected;
    }
}
