using UnityEngine;
using System.Collections.Generic;


public enum eHand
{
    Nothing,
    One_Pair,
    Two_Pairs,
    Three_Kind,
    Straight,
    Flush,
    Full_House,
    Four_of_a_Kind
}


public class JackpotsHands 
{
    private int m_SpadeSum;
    private int m_ClubSum;
    private int m_HeartSum;
    private int m_DiamondSum;

    private Card m_HighCard;
    public Card _HighCard { get { return m_HighCard; } }

    private List<Card> m_EvaluateList;


    public JackpotsHands()
    {
        m_SpadeSum = 0;
        m_ClubSum = 0;
        m_HeartSum = 0;
        m_DiamondSum = 0;

        m_HighCard = null;

        m_EvaluateList = new List<Card>();
    }

    public eHand EvaluateHand(List<Card> a_Cards)
    {
        m_EvaluateList.Clear();
        m_EvaluateList.AddRange(a_Cards);

        if (m_EvaluateList.Count != 5)
        {
            Debug.LogError("An Invalid amount of cards has been sent to evaluate");
            return eHand.Nothing;
        }

        m_EvaluateList.Sort(SortHandByScore);
        m_HighCard = m_EvaluateList[m_EvaluateList.Count - 1];

        eHand resultHand = eHand.Nothing;

        CalculateSuitSums(m_EvaluateList);
        if (FourOfKind(m_EvaluateList))
            return eHand.Four_of_a_Kind;
        else if (FullHouse(m_EvaluateList))
            return eHand.Full_House;
        else if (Flush(m_EvaluateList))
            return eHand.Flush;
        else if (Straight(m_EvaluateList))
            return eHand.Straight;
        else if (ThreeOfKind(m_EvaluateList))
            return eHand.Three_Kind;
        else if (TwoPairs(m_EvaluateList))
            return eHand.Two_Pairs;
        else if (OnePair(m_EvaluateList))
            return eHand.One_Pair;
 
        return resultHand;
    }

    private int SortHandByScore(Card a_Card1, Card a_Card2)
    {
        int score = a_Card1._Score - a_Card2._Score;
        return Mathf.Clamp(score, -1, 1);
    }

    private void CalculateSuitSums(List<Card> a_Cards)
    {
        ResetSuitScores(); 
        for (int i = 0; i < a_Cards.Count; i++)
        {
            switch(a_Cards[i]._Sign)
            {
                case eCardSign.Spade:
                    m_SpadeSum++;
                    break;

                case eCardSign.Club:
                    m_ClubSum++;
                    break;

                case eCardSign.Heart:
                    m_HeartSum++;
                    break;

                case eCardSign.Diamond:
                    m_DiamondSum++;
                    break;
            }
        }
    }

    private void ResetSuitScores()
    {
        m_SpadeSum = 0;
        m_ClubSum = 0;
        m_HeartSum = 0;
        m_DiamondSum = 0;

        m_HighCard = null;
    }

    #region Hand Evaluation Conditions
    private bool FourOfKind(List<Card> a_Cards)
    {
        //if the first 4 cards, and values of the four cards and last card is the highest
        if (a_Cards[0]._Score == a_Cards[1]._Score && a_Cards[0]._Score == a_Cards[2]._Score && a_Cards[0]._Score == a_Cards[3]._Score)
        {
            return true;
        }
        else if (a_Cards[1]._Score == a_Cards[2]._Score && a_Cards[1]._Score == a_Cards[3]._Score && a_Cards[1]._Score == a_Cards[4]._Score)
        {
            return true;
        }

        return false;
    }

    private bool FullHouse(List<Card> a_Cards)
    {
        //the first three cars and last two cards are of the same value
        //first two cards, and last three cards are of the same value
        if ((a_Cards[0]._Score == a_Cards[1]._Score && a_Cards[0]._Score == a_Cards[2]._Score && a_Cards[3]._Score == a_Cards[4]._Score) ||
            (a_Cards[0]._Score == a_Cards[1]._Score && a_Cards[2]._Score == a_Cards[3]._Score && a_Cards[2]._Score == a_Cards[4]._Score))
        {
            return true;
        }

        return false;
    }

    private bool Flush(List<Card> a_Cards)
    {
        //if all suits are the same
        if (m_SpadeSum == 5 || m_ClubSum == 5 || m_HeartSum == 5 || m_DiamondSum == 5)
        {
            //if flush, the player with higher cards win
            //whoever has the last card the highest, has automatically all the cards total higher
            return true;
        }

        return false;
    }

    private bool Straight(List<Card> a_Cards)
    {
        //if 5 consecutive values
        if (a_Cards[0]._Score + 1 == a_Cards[1]._Score &&
            a_Cards[1]._Score + 1 == a_Cards[2]._Score &&
            a_Cards[2]._Score + 1 == a_Cards[3]._Score &&
            a_Cards[3]._Score + 1 == a_Cards[4]._Score)
        {
            //player with the highest value of the last card wins
            return true;
        }

        return false;
    }

    private bool ThreeOfKind(List<Card> a_Cards)
    {
        //if the 1,2,3 cards are the same OR
        //2,3,4 cards are the same OR
        //3,4,5 cards are the same
        //3rds card will always be a part of Three of A Kind
        if ((a_Cards[0]._Score == a_Cards[1]._Score && a_Cards[0]._Score == a_Cards[2]._Score) ||
        (a_Cards[1]._Score == a_Cards[2]._Score && a_Cards[1]._Score == a_Cards[3]._Score))
        {
            return true;
        }
        else if (a_Cards[2]._Score == a_Cards[3]._Score && a_Cards[2]._Score == a_Cards[4]._Score)
        {
            return true;
        }
        return false;
    }

    private bool TwoPairs(List<Card> a_Cards)
    {
        //if 1,2 and 3,4
        //if 1.2 and 4,5
        //if 2.3 and 4,5
        //with two pairs, the 2nd card will always be a part of one pair 
        //and 4th card will always be a part of second pair
        if (a_Cards[0]._Score == a_Cards[1]._Score && a_Cards[2]._Score == a_Cards[3]._Score)
        {
            return true;
        }
        else if (a_Cards[0]._Score == a_Cards[1]._Score && a_Cards[3]._Score == a_Cards[4]._Score)
        {
            return true;
        }
        else if (a_Cards[1]._Score == a_Cards[2]._Score && a_Cards[3]._Score == a_Cards[4]._Score)
        {
            return true;
        }
        return false;
    }

    private bool OnePair(List<Card> a_Cards)
    {
        //if 1,2 -> 5th card has the highest value
        //2.3
        //3,4
        //4,5 -> card #3 has the highest value
        if (a_Cards[0]._Score == a_Cards[1]._Score)
        {
            return true;
        }
        else if (a_Cards[1]._Score == a_Cards[2]._Score)
        {
            return true;
        }
        else if (a_Cards[2]._Score == a_Cards[3]._Score)
        {
            return true;
        }
        else if (a_Cards[3]._Score == a_Cards[4]._Score)
        {
            return true;
        }

        return false;
    }
    #endregion
}
