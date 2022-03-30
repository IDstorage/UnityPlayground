using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UP02
{
    public class Ping : MonoBehaviour
    {
        public static List<Ping> pingList = new List<Ping>();
        public static UnityEngine.Events.UnityEvent onPingUpdated;

        [SerializeField]
        private Renderer targetRenderer;
        public bool IsVisible => targetRenderer.isVisible;

        private void OnEnable()
        {
            pingList.Add(this);
            onPingUpdated?.Invoke();
        }
        private void OnDisable()
        {
            pingList.Remove(this);
            onPingUpdated?.Invoke();
        }
    }
}