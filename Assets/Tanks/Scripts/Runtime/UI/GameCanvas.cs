using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaltedFishStudio.RoadKill.UI
{
    public class GameCanvas : MonoBehaviour, ICanvas
    {
       
        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}