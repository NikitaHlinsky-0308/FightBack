using UnityEngine;

namespace CodeBase.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        private Transform _following;
        
        
        [Header("Camera Follow")][Space]
        [Range(min:0, max:100)][SerializeField] private float _rotationAngleX = 60f;
        [SerializeField] private Vector3 _distance = new Vector3(0f, 7f, -4f);
        [Range(min:0, max:10)][SerializeField] private float _smoothness = 1f;
        
        
        private void Update() => Move();

        public void Follow(Transform following) => _following = following.transform;


        
        
        private void Move()
        {
            Vector3 nextPosition = Vector3.Lerp(
                transform.position, 
                _following.position + _distance, 
                Time.deltaTime * _smoothness);
            Quaternion rotation = Quaternion.Euler(_rotationAngleX, 0, 0);
            
            transform.rotation = rotation;
            transform.position = nextPosition;
        }
        
    }
}