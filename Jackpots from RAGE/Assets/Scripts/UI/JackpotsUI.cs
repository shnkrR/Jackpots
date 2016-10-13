using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class JackpotsUI : MonoBehaviour 
{
    [SerializeField]
    private Image _CardSign;

    [SerializeField]
    private Text _TopLeftCardText;
    [SerializeField]
    private Text _BottomRightCardText;

    [SerializeField]
    private CardUI[] _SignSprites;


    private void Start()
    {

    }

    public void InitCard(Card a_Card)
    {
        Sprite sprite = _SignSprites[0]._CardSignSprite;
        Color color = Color.black;

        for (int i = 0; i < _SignSprites.Length; i++)
        {
            if (_SignSprites[i]._CardSign == a_Card._Sign)
            {
                sprite = _SignSprites[i]._CardSignSprite;
                color = _SignSprites[i]._CardTextColor;
                break;
            }
        }

        _CardSign.sprite = sprite;
        _TopLeftCardText.text = _BottomRightCardText.text = a_Card._Number;
        _TopLeftCardText.color = _BottomRightCardText.color = color;
    }

    public bool IsHeld()
    {
        return gameObject.GetComponent<Toggle>().isOn;
    }

    public void ResetHold()
    {
        gameObject.GetComponent<Toggle>().isOn = false;
    }
}
