using UnityEngine;
using UnityEngine.UI;
public class TextField : MonoBehaviour {

    private string textValue;
    private Color textColor;
    public Text textElement;

    public TextField(string _textValue, Color _textColor) {
        this.textValue = _textValue;
        this.textColor = _textColor;
    }

    void Start() {
        textElement.text = textValue;
        textElement.color = textColor;
    }

    void Update() {
        
    }
}