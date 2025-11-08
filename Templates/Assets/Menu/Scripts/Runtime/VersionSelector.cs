using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class VersionSelector : MonoBehaviour
{
    [SerializeField] private string _versionKey = "{ver}";
    [SerializeField] private string _companyKey = "{company}";
    [SerializeField] private TMP_Text _text;

    void Start()
    {
        DisplayVersion();
    }
    public async void DisplayVersion()
    {
        await Task.Delay(40);
        _text.text = _text.text.Replace(_versionKey, Application.version).Replace(_companyKey, Application.companyName);
    }
}
