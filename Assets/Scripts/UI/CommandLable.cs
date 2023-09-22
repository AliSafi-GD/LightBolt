using UnityEngine;
using UnityEngine.UI;

    public class CommandLable : MonoBehaviour
    {
        [SerializeField] private Image imgIcon;
        [SerializeField] private Image imgBg;
        [SerializeField] private Color acceptColor, rejectColor;
        public void SetSprite(Sprite sprite)
        {
            imgIcon.sprite = sprite;
        }

        public void SetStatus(TaskStatus status)
        {
            imgBg.color = status == TaskStatus.Accept ? acceptColor : rejectColor;
        }
    }
