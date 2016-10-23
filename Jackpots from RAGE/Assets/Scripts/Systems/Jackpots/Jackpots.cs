using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Jackpots : JackpotsBase
{
    public GameObject _CardPrefab;
    public GameObject _Grid;

    public JackpotsUI _UI;

    public JackpotsRewards[] _Rewards = new JackpotsRewards[(int)eHand.Max];

    private List<JackpotsCardUI> m_JackpotsUI = new List<JackpotsCardUI>();

    private JackpotsHands m_HandEvaluator;

    private const int m_CardCount = 5;
    private const int m_CardBounds = 480;
    private const int m_CardWidth = 120;
    private const int m_MaxTurns = 5;
    
    private int m_CurrTurn = 1;

    private List<Card> m_ActiveCards = new List<Card>();

    private int m_CurrentBet;


    protected override void Start()
    {
        base.Start();

        if (m_HandEvaluator == null)
            m_HandEvaluator = new JackpotsHands();

        Init();
    }

    private void Reset()
    {
        base.Start();

        for (int i = 0; i < m_JackpotsUI.Count; i++)
        {
            Destroy(m_JackpotsUI[i].gameObject);
        }

        Init();

        _UI.Start();
    }

    private void Init()
    {
        m_JackpotsUI.Clear();
        m_ActiveCards.Clear();
        m_ActiveCards.AddRange(GetCards(m_CardCount));
        for (int i = 0; i < m_ActiveCards.Count; i++)
        {
            GameObject go = Instantiate(_CardPrefab);
            go.transform.SetParent(_Grid.transform, false);
            m_JackpotsUI.Add(go.GetComponent<JackpotsCardUI>());
            m_JackpotsUI[i].InitCard(m_ActiveCards[i]);
            m_JackpotsUI[i].ResetHold();
            go = null;
        }

        HorizontalLayoutGroup hlg = _Grid.GetComponent<HorizontalLayoutGroup>();
        hlg.spacing = (m_CardBounds - (m_CardWidth * m_CardCount)) / m_CardCount;

        m_CurrTurn = 1;
        _UI.UpdateResult(m_HandEvaluator.EvaluateHand(m_ActiveCards));
    }

    protected override void EndGame()
    {
        base.EndGame();

        eHand resultHand = m_HandEvaluator.EvaluateHand(m_ActiveCards);
        float betMultiplier = 0;
        float winnings = 0;

        for (int i = 0; i < _Rewards.Length; i++)
        {
            if (_Rewards[i]._ResultHand == resultHand)
            {
                betMultiplier = _Rewards[i]._BetMultiplier;
                break;
            }
        }
        winnings = (betMultiplier * m_CurrentBet);

        _UI.UpdateResult(resultHand, winnings, true);
        m_Wallet += winnings;
        PlayerPrefs.SetFloat(m_WalletPrefsKey, m_Wallet);
    }

    protected override void UpdateWallet(float a_Amount)
    {
        base.UpdateWallet(a_Amount);

        _UI.SetWallet(m_Wallet);
    }

    public void OnDrawPressed()
    {
        int length = m_ActiveCards.Count;
        for (int i = 0; i < length; i++)
        {
            if (!m_JackpotsUI[i].IsHeld())
            {
                DiscardCards(m_ActiveCards[i]);
                m_ActiveCards[i] = GetCards(1)[0];
                //m_JackpotsUI[i].InitCard(m_ActiveCards[i]);
            }

            m_JackpotsUI[i].InitCard(m_ActiveCards[i]);
            m_JackpotsUI[i].ResetHold();
        }

        _UI.UpdateResult(m_HandEvaluator.EvaluateHand(m_ActiveCards));
        m_CurrTurn++;
        if (m_CurrTurn > m_MaxTurns)
        {
            EndGame();
        }
    }

    public void OnRestart()
    {
        Reset();
    }

    public void OnCashOut()
    {
        m_CurrTurn = m_MaxTurns + 1;
        EndGame();
    }

    public void OnApplyBet()
    {
        if (_UI._CurrentBetAmount <= m_Wallet)
        {
            m_CurrentBet = _UI._CurrentBetAmount;
            m_Wallet -= m_CurrentBet;
            _UI.ShowGame();
        }
    }
}
