using TMPro;
using UnityEngine;

public class ResourseCountView : MonoBehaviour
{
    [SerializeField] private Storage _storage;
    [SerializeField] private TextMeshProUGUI _count;

    private void OnEnable()
    {
        _storage.Updated += ViewCount;
    }

    private void OnDisable()
    {
        _storage.Updated -= ViewCount;
    }

    private void ViewCount(int count)
    {
        _count.text = count.ToString(); 
    }
}
