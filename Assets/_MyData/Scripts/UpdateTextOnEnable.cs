using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTextOnEnable : MonoBehaviour
{
    public string key;
    public bool isInt;
    public bool isFloat;
    public bool isString;
    ///
    Text txt;
    ///
    private void Awake() {
        txt = GetComponent<Text>();
    }
    private void Start() {
        OnEnable();
    }
    private void OnEnable() {
        if(isInt) txt.text = PlayerPrefs.GetInt(key).ToString();
        if(isFloat) txt.text = PlayerPrefs.GetFloat(key).ToString();
        if(isString) txt.text = PlayerPrefs.GetString(key);
    }
    ///
    public void UpdateValue() {
        OnEnable();
    }
}
