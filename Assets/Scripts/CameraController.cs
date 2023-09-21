using UnityEngine;

namespace STUDENT_NAME
{
    public class CameraController : SimpleGameStateObserver
    {
        [SerializeField] private Transform m_Target;
        private Vector3 m_InitPosition;
        private Transform m_Transform;

        protected override void Awake()
        {
            base.Awake();
            m_Transform = transform;
            m_InitPosition = m_Transform.position;
        }

        private void Update()
        {
            if (!GameManager.Instance.IsPlaying) return;

            // TO DO
        }

        private void ResetCamera()
        {
            m_Transform.position = m_InitPosition;
        }

        protected override void GameMenu(GameMenuEvent e)
        {
            ResetCamera();
        }
    }
}