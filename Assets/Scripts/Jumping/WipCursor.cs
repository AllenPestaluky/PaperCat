using UnityEngine;

namespace CatGame.Movement
{
    public class WipCursor : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer _renderer;

        public void UpdatePosition(int x, int z, float y)
        {

        }

        public void Toggle(bool show)
        {
            _renderer.enabled = show;
        }
    }

}
