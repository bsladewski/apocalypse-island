using System;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    private Outline outline;

    private bool isSelected;

    public bool GetIsSelected()
    {
        return isSelected;
    }

    private void Start()
    {
        InteractionSystem.Instance.OnSelect += OnSelect;
        InteractionSystem.Instance.OnDeselect += OnDeselect;
    }

    private void OnDestroy()
    {
        InteractionSystem.Instance.OnSelect -= OnSelect;
        InteractionSystem.Instance.OnDeselect -= OnDeselect;
    }

    private void OnSelect(object sender, GameObject go)
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

    private void OnDeselect(object sender, EventArgs e)
    {
        if (isSelected)
        {
            SetIsSelected(false);
        }
    }

    private void SetIsSelected(bool isSelected)
    {
        if (outline != null)
        {
            outline.enabled = isSelected;
        }
        else
        {
            Debug.LogWarning("Selectable object has no outline assigned!");
        }

        this.isSelected = isSelected;
    }
}
