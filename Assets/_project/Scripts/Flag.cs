using UnityEngine;

public class Flag : MonoBehaviour
{
    private bool _selected;

    public bool Selected => _selected;

    public void MakeSelected()
    {
        _selected = true;
    }

    public void MakeUnselected()
    {
        _selected = false;
    }
}
