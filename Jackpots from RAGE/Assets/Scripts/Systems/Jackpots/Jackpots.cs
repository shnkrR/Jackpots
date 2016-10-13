using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Jackpots : JackpotsBase
{
    public GameObject _CardPrefab;
    public GameObject _Grid;

    private List<JackpotsUI> m_JackpotsUI = new List<JackpotsUI>();

    private JackpotsHands m_HandEvaluator;

    private int m_CardCount = 5;
    private int m_CardBounds = 480;
    private int m_CardWidth = 120;

    private List<Card> m_ActiveCards = new List<Card>();


    protected override void Start()
    {
        base.Start();

        m_HandEvaluator = new JackpotsHands();
        m_ActiveCards.AddRange(GetCards(m_CardCount));
        for (int i = 0; i < m_ActiveCards.Count; i++)
        {
            GameObject go = Instantiate(_CardPrefab);
            go.transform.SetParent(_Grid.transform, false);
            m_JackpotsUI.Add(go.GetComponent<JackpotsUI>());
            m_JackpotsUI[i].InitCard(m_ActiveCards[i]);
            m_JackpotsUI[i].ResetHold();
            go = null;
        }

        HorizontalLayoutGroup hlg = _Grid.GetComponent<HorizontalLayoutGroup>();
        hlg.spacing = (m_CardBounds - (m_CardWidth * m_CardCount)) / m_CardCount;
    }

    //TODO: There is a bug here that causes draw to not work properly after an evaluate for some reason. Needs fix
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
    }

    public void OnEvaluatePressed()
    {
        Debug.Log(m_HandEvaluator.EvaluateHand(m_ActiveCards));
    }
}
