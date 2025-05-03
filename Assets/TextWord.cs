using UnityEngine;
using UnityEngine.UI;

public class TextWord : MonoBehaviour
{
    [SerializeField] private Text wordText;
    public void UpdateTextWord(string currentWord)
    {
        wordText.text = currentWord;
    }
}
