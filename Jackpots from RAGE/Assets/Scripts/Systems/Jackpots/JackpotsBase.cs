using UnityEngine;
using System.Collections.Generic;


public enum eCardSign
{
    Spade = 0,
    Club,
    Heart,
    Diamond,
    Max,
}

public class Card
{
    public string _Number;
    public eCardSign _Sign;
    public int _Score;

    public Card()
    {
        _Number = "A";
        _Sign = eCardSign.Spade;
        _Score = 0;
    }

    public Card(string a_Number, eCardSign a_Sign, int a_Score)
    {
        _Number = a_Number;
        _Sign = a_Sign;
        _Score = a_Score;
    }
}

[System.Serializable]
public class CardUI
{
    public eCardSign _CardSign;

    public Sprite _CardSignSprite;

    public Color _CardTextColor;
}

public class JackpotsBase : MonoBehaviour 
{
    private const int m_NumCards = 52;

    private List<Card> m_CardDeck = new List<Card>(m_NumCards);

    protected const string m_WalletPrefsKey = "PREF_WALLET";

    protected float m_Wallet { get { return m_privateWallet; } set { UpdateWallet(value); } }
    private float m_privateWallet;


    protected virtual void Start()
    {
        if (m_CardDeck.Count != m_NumCards)
        {
            m_CardDeck.Clear();
            int signs = (int)eCardSign.Max;
            for (int i = 0; i < signs; i++)
            {
                for (int j = 0; j < (m_CardDeck.Capacity / signs); j++)
                {
                    string card = "";
                    int score = j;
                    if (j == 0)
                    {
                        card = "A";
                        score = (m_CardDeck.Capacity / signs);
                    }
                    else if ((j % 10) == 0)
                        card = "J";
                    else if ((j % 11) == 0)
                        card = "Q";
                    else if ((j % 12) == 0)
                        card = "K";
                    else
                        card = "" + (j + 1);

                    m_CardDeck.Add(new Card(card, ((eCardSign)i), score));
                }
            }
        }

        m_CardDeck.Shuffle();
        m_Wallet = PlayerPrefs.GetFloat(m_WalletPrefsKey, 100);
    }

    protected virtual void EndGame()
    {

    }

    protected virtual void UpdateWallet(float a_Amount)
    {
        m_privateWallet = a_Amount;
        PlayerPrefs.SetFloat(m_WalletPrefsKey, m_privateWallet);
    }

    protected List<Card> GetCards(int count)
    {
        List<Card> result = new List<Card>();
        count = Mathf.Clamp(count, 0, m_CardDeck.Count);

        for (int i = 0; i < count; i++)
        {
            Card card = m_CardDeck[i];
            result.Add(card);
            m_CardDeck.Remove(card);
        }

        return result;
    }

    protected void DiscardCards(params Card[] a_Cards)
    {
        m_CardDeck.AddRange(a_Cards);
    }
}
