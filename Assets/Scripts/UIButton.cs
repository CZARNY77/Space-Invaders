using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] GameObject endPanel;
    [SerializeField] bool restart;

    public void OnPointerUp(PointerEventData eventData)
    {
        endPanel.GetComponent<Animator>().SetTrigger("restart");
        if(restart)
            Invoke(nameof(NewGame), 1);
        else
            Invoke(nameof(EndPanel), 1);
    }

    void NewGame()
    {
        GameManager.instance.NewGame();
    }
    void EndPanel()
    {
        GameManager.instance.NewHighScore();
    }
}
