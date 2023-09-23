using Levels;
using UnityEngine;

namespace Public
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    var rs = Resources.Load<ResourceManager>("ResourceManager");
                    instance = Instantiate(rs);
                }

                return instance;
            }
        }
        private static ResourceManager instance;

        public LevelsData Levels;
    }
}