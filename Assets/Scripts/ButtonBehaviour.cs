using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class ButtonBehaviour : MonoBehaviour
{
    NPCStateManager _context;

    Vector3 _defaulScale = Vector3.one;
    Vector3 _increasedScale = new Vector3(1.4f, 1.4f, 1.4f);
    Vector3 _selectedScale = new Vector3(1.8f, 1.8f, 1.8f);
    Image _image;
    TextMeshProUGUI _textMesh;
    private void Start()
    {
        _context = GetComponentInParent<NPCStateManager>();

        _image = GetComponent<Image>();
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void OnMouse()
    {
        this.transform.DOScale(_increasedScale, 0.3f);
        _image.color = Color.yellow;
        _textMesh.color = Color.black;
    }
    public void NotOnMouse()
    {
        this.transform.DOScale(_defaulScale, 0.3f);
        _image.color = Color.black;
        _textMesh.color = Color.white;
    }
    public void OnMouseClick()
    {
        this.transform.DOScale(_selectedScale, 0.3f);
        _image.color = Color.white;
        _textMesh.color = Color.white;

        if(this.gameObject.tag == "Button_1")
        {
            _context.Talk(0);

        }
        else if(this.gameObject.tag == "Button_2")
        {
            _context.Talk(1);
        }
    }
}
