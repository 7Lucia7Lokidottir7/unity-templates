using UnityEngine;
namespace PG.BattleSystem
{
    public class TurnToEnemyBehaviour : StateMachineBehaviour
    {
        [SerializeField] private string _tag = "Enemy";
        [SerializeField, Range(0, 1)] private float _startTimeInvoke = 0.1f;
        [SerializeField, Range(0, 1)] private float _stopTimeInvoke = 0.35f;
        [SerializeField] private float _speed = 4f;
        private GameObject _enemyGameObject;
        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(_tag);
            float closestDistanceSq = Mathf.Infinity;
            GameObject closestGameObject = null;
            foreach (GameObject gameObject in gameObjects)
            {
                float distanceSq = Vector3.SqrMagnitude(animator.transform.position - gameObject.transform.position);
                if (closestGameObject == null || distanceSq < closestDistanceSq)
                {
                    closestDistanceSq = distanceSq;
                    closestGameObject = gameObject;
                    _enemyGameObject = closestGameObject;
                }
            }
        }

        // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime > _startTimeInvoke && stateInfo.normalizedTime < _stopTimeInvoke)
            {
                Vector3 direction = (_enemyGameObject.transform.position - animator.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                animator.transform.rotation = Quaternion.Lerp(animator.transform.rotation, lookRotation, _speed * Time.deltaTime);
            }
        }

        // OnStateExit is called before OnStateExit is called on any state inside this state machine
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called before OnStateMove is called on any state inside this state machine
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateIK is called before OnStateIK is called on any state inside this state machine
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMachineEnter is called when entering a state machine via its Entry Node
        //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        //{
        //    
        //}

        // OnStateMachineExit is called when exiting a state machine via its Exit Node
        //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        //{
        //    
        //}
    }
}
