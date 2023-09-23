
    using System.Collections.Generic;
    using UnityEngine;

    public class CharacterLogic
    {
        public CharacterLogic(ICharacterData data, ICharacterView view)
        {
            characterData = data;
            characterView = view;
        }
        private ICharacterView characterView;
        private ICharacterData characterData;
        public List<GroundItem> grounds = new List<GroundItem>();
        public void FindForwardGround()
        {
            foreach (var groundItem in grounds)
            {
                if(groundItem.Equals(characterData.currentGround))
                    continue;
            
            
                var targetPos = groundItem.position;
                targetPos.y = 0;

                var playerPos = characterView.Transform.position;
                playerPos.y = 0;
                Vector3 targetDir =targetPos - playerPos;
                float angle = Vector3.Angle(targetDir, characterView.Transform.forward);
//            Debug.Log(groundItem.name+"     "+angle+"       dis : "+Vector3.Distance(targetPos,playerPos));
                if (angle < 10 && Vector3.Distance(targetPos,playerPos)<=1.3f)
                {
                    characterData.forwardGround = groundItem;
                    return;
                }
           
            }

            characterData.forwardGround = null;
        }
    }
