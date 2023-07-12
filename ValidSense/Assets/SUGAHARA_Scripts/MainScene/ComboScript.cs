using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboScript : MonoBehaviour
{
    /// <summary>
    /// コンボ数表記用
    /// </summary>
    Text comboValueText;


    [SerializeField]
    Text comboText;

    /// <summary>
    /// コンボ数
    /// </summary>
    public int comboValue;

    // Start is called before the first frame update
    void Start()
    {
        comboValueText = GetComponent<Text>();

        _ComboValueUpdate();
    }


    /// <summary>
    /// コンボ数を加算
    /// </summary>
    public void AddComboValue()
    {
        comboValue++;

        _ComboValueUpdate();
    }

    /// <summary>
    /// コンボ数をリセット
    /// </summary>
    public void ComboReset()
    {
        comboValue = 0;

        _ComboValueUpdate();
    }

    /// <summary>
    /// コンボ数の表記用
    /// </summary>
    private void _ComboValueUpdate()
    {
        comboValueText.text = comboValue.ToString();
    }

    

    public void HideComboText()
    {
        comboValueText.color -= new Color(0, 0, 0, 1f);
        comboText.color -= new Color(0, 0, 0, 1f);
    }
}
