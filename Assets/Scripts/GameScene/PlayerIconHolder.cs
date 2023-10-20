using UnityEngine;
using UnityEngine.UI;

public class PlayerIconHolder : MonoBehaviour
{
    [SerializeField] Image playerIcon;

    public void SetPlayerIcon(Sprite icon)
    {
        playerIcon.sprite = icon;
    }

    public void SetPlayerIcon(int index)
    {
        playerIcon.sprite = FindObjectOfType<PlayerIconsList>().GetIcon(index);
    }
}
