using System.Threading.Tasks;
using UnityEngine;

namespace PG.BattleSystem
{
    public class TurnToCameraBehaviour : StateMachineBehaviour
    {
        [SerializeField] private float _duration = 0.1f;
        private Camera _camera;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _camera = Camera.main;
            OnTurn(animator);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            _camera = Camera.main;
            OnTurn(animator);
        }
        async void OnTurn(Animator animator)
        {
            for (float i = 0; i < _duration; i += Time.deltaTime)
            {
                animator.transform.rotation = Quaternion.Lerp(animator.transform.rotation, Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0), i / _duration);
                await Task.Yield();
            }
        }
    }
}
