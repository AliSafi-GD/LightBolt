using System.Collections.Generic;
using UnityEngine;


namespace Ground
{
    [System.Serializable]

    public class GroundsHelper
    {
        [SerializeField] List<GroundItem> grounds = new List<GroundItem>();

        public GroundsHelper(List<GroundItem> grounds)
        {
            this.grounds = grounds;
        }

        public GroundItem FindForwardGround(GroundItem currentGround, Transform characterView)
        {
            foreach (var groundItem in grounds)
            {
                if (groundItem.Equals(currentGround))
                    continue;


                var targetPos = groundItem.position;
                targetPos.y = 0;

                var playerPos = characterView.position;
                playerPos.y = 0;
                Vector3 targetDir = targetPos - playerPos;
                float angle = Vector3.Angle(targetDir, characterView.forward);
                if (angle < 10 && Vector3.Distance(targetPos, playerPos) <= 1.3f)
                {
                    return groundItem;
                }

            }

            return null;
        }
    }
}
