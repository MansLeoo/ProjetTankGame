using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int index ; // Index du panel

    public void OnPointerClick(PointerEventData eventData)
    {
        // Informer le UIManager qu'un panel a été cliqué
        UiManager.Instance.OnPanelClicked(this.gameObject,this.index);
    }

}

