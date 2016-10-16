using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JackpotsUI : MonoBehaviour 
{
    public GameObject _RestartButton;
    public GameObject _DrawButton;

    public Text _ResultHand;


    public void Start()
    {
        _RestartButton.SetActive(false);
        _DrawButton.SetActive(true);
    }

    public void UpdateResult(eHand a_ResultHand, bool a_AllowRestart = false)
    {
        _RestartButton.SetActive(a_AllowRestart);
        _DrawButton.SetActive(!a_AllowRestart);
        _ResultHand.text = ((a_AllowRestart) ? "Final" : "Current") + " Hand: " + a_ResultHand.ToString().Replace("_", " ");
    }
}
