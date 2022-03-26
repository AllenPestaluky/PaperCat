using UnityEngine;

namespace CatGame.Movement
{
    public class TubeOfJumpiness : MonoBehaviour
    {
        [SerializeField]
        private float _diameter;

        private void OnValidate()
        {
            gameObject.transform.localScale = new Vector3(_diameter, 8f, _diameter);
        }
    }

}
