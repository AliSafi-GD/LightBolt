using System;
using UnityEngine;
using UnityEngine.UI;

    public class CommandLable : MonoBehaviour
    {
        [SerializeField] private Image imgIcon;
        [SerializeField] private Image imgBg;
        [SerializeField] private Color acceptColor, rejectColor;
        private Button btn;
        private GameManager mamanger;
        public ICommand _command { get; private set; }
        private void Start()
        {
            btn = GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                mamanger.RemoveCommand(_command);
            });
        }

        public void Init(GameManager manager,ICommand command)
        {
            _command = command;
            this.mamanger = manager;
        }

        public void SetSprite(Sprite sprite)
        {
            imgIcon.sprite = sprite;
        }

        // public void SetStatus(TaskStatus status)
        // {
        //     imgBg.color = status == TaskStatus.Accept ? acceptColor : rejectColor;
        // }
    }
