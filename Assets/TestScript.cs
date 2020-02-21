using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    Color textColor;
    [SerializeField]
    Color backgroundColor;

    [SerializeField]
    [Range(1f, 21f)]
    float targetRatio = 4.5f;

    [SerializeField]
    Text text;
    [SerializeField]
    Image backgroundImage;

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        text.color = textColor;
        backgroundImage.color = backgroundColor;
    }

    public void Apply()
    {
        text.color = text.color.EnsureContrastRatio(backgroundImage.color, targetRatio);
    }


}
