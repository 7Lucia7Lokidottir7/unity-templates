using UnityEngine;
namespace PG.LocomotionSystem
{
    public class LocomotionEnableStateBehaviour : StateMachineBehaviour
    {
        private ThirdPersonMovement _thirdPersonMovement;
        [SerializeField] private bool _enabled;
        [SerializeField] private float _minTime = 0f;
        [SerializeField] private float _maxTime = 1f;
        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            _thirdPersonMovement = animator.GetComponentInChildren<ThirdPersonMovement>();
        }

        // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _thirdPersonMovement.enabled = stateInfo.normalizedTime >= _minTime && stateInfo.normalizedTime < _maxTime ? _enabled : !_enabled;
        }

        // OnStateExit is called before OnStateExit is called on any state inside this state machine
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _thirdPersonMovement.enabled = !_enabled;
        }

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
        //}

        // OnStateMachineExit is called when exiting a state machine via its Exit Node
        //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        //{
        //}
    }
}

