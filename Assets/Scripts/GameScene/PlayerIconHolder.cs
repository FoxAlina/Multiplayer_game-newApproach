using UnityEngine;
using UnityEngine.UI;

public class PlayerIconHolder : MonoBehaviour
{
    [SerializeField] Image playerIcon;

    public void SetPlayerIcon(Sprite icon)
    {
        playerIcon.sprite = icon;
    }
}
