using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isHoldingLeft = false;
    public bool isHoldingRight = false;
    public int LeftOrRight = 0;

    public void OnPointerDown(PointerEventData eventData)
    {

        if (LeftOrRight == 0)
        {
            isHoldingLeft = true;
        }
        else
        {
            isHoldingRight = true;
        }
        Debug.Log("Basılı tutuluyor");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (LeftOrRight == 0)
        {
            isHoldingLeft = false;
        }
        else
        {
            isHoldingRight = false;
        }
        Debug.Log("Buton bırakıldı");
    }

    void Update()
    {
        if (isHoldingLeft)
        {
            // Sürekli yapılan şeyler buraya
            Debug.Log("Hala basılıyor...");
        }
    }
}
