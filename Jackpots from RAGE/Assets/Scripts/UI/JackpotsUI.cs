using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JackpotsUI : MonoBehaviour 
{
    public GameObject _RestartButton;
    public GameObject _DrawButton;
    public GameObject _BetPanel;
    public GameObject _CardPanel;
    public GameObject _CashOutButton;

    public Text _ResultHand;
    public Text _CurrentBet;
    public Text _Wallet;
    public Text _Winnings;

    [HideInInspector]public int _CurrentBetAmount;


    public void Start()
    {
        _RestartButton.SetActive(false);
        _BetPanel.SetActive(true);
        _DrawButton.SetActive(true);
        _CardPanel.SetActive(false);

        _Winnings.gameObject.SetActive(false);

        _CurrentBet.text = "Bet: 0$";
    }

    public void ShowGame()
    {
        _RestartButton.SetActive(false);
        _CashOutButton.SetActive(false);
        _BetPanel.SetActive(false);
        _DrawButton.SetActive(true);
        _CardPanel.SetActive(true);
    }

    public void UpdateResult(eHand a_ResultHand, float a_WinAmount = 0, bool a_AllowRestart = false)
    {
        _CashOutButton.SetActive(!a_AllowRestart);
        _RestartButton.SetActive(a_AllowRestart);
        _DrawButton.SetActive(!a_AllowRestart);
        _ResultHand.text = ((a_AllowRestart) ? "Final" : "Current") + " Hand: " + a_ResultHand.ToString().Replace("_", " ");
        _Winnings.gameObject.SetActive(a_AllowRestart);
        _Winnings.text = "Winnings: " + a_WinAmount + "$";
    }

    public void SetBet(int a_BetAmount)
    {
        _CurrentBet.text = "Bet: " + a_BetAmount + "$";
        _CurrentBetAmount = a_BetAmount;
    }

    public void SetWallet(float a_Amount)
    {
        _Wallet.text = "Wallet: " + a_Amount + "$";
    }
}
