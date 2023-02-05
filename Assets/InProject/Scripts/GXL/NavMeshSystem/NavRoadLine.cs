using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using LitJson;
public class NavRoadLine: MonoBehaviour {
    private NavMeshAgent agent;
    private LineRenderer lineRenderer;
    [SerializeField] private Transform target;

    private void Start() {
        agent = GetComponent < NavMeshAgent > ();
        agent.updateRotation = false;
        lineRenderer = GetComponentInChildren < LineRenderer > ();
    }


    private void Update() {
        WaitFrame();
    }

    void WaitFrame() {
        if (Time.frameCount % 2 == 0) {
            if (EventSystem.current.IsPointerOverGameObject()) //判断当前鼠标是否点击物体上
            {
                Debug.Log(true);
                return;
            }

            agent.SetDestination(target.position);
            var corners = agent.path.corners;
            lineRenderer.positionCount = corners.Length;
            lineRenderer.SetPositions(corners);
             agent.isStopped = true;
            // Debug.Log(corners.Length);
        }
    }
}