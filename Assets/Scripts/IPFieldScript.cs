using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using Button = UnityEngine.UI.Button;

public class IPFieldScript : MonoBehaviour {
    private InputField m_input;
    private bool m_selfChanged = false;

    void Start()
    {
        m_input = GetComponent<InputField>();
        m_input.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<string>((str) => OnTextChanged(str)));
    }

    void OnTextChanged(string _value)
    {
        if (m_selfChanged)
            return;

        m_selfChanged = true;
        if (! (char.IsDigit(_value[_value.Length - 1]) || _value[_value.Length-1] == '.'))
            _value.Remove(_value.Length - 1);

        m_input.text = _value;


        m_selfChanged = false;
    }
	
}
