using UnityEngine;
namespace PG.VFXControl
{
    public class VisualEffectInvokeBehaviour : StateMachineBehaviour
    {
        private VisualEffectInvoker _visualEffectInvoker;
        [SerializeField, Range(0, 1)] private float _startTimeInvoke = 0.25f;
        [SerializeField, Range(0, 1)] private float _stopTimeInvoke = 0.75f;
        [SerializeField] private int _visualEffectIndex;
        [SerializeField] private bool _isInvokeOnce;

        private bool _isInvoked;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _visualEffectInvoker = animator.GetComponentInChildren<VisualEffectInvoker>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_isInvokeOnce)
            {
                if (!_isInvoked)
                {
                    if (stateInfo.normalizedTime > _startTimeInvoke)
                    {
                        _isInvoked = true;
                        _visualEffectInvoker.Play(_visualEffectIndex);
                    }
                }
            }
            else
            {
                if (!_isInvoked)
                {
                    if (stateInfo.normalizedTime > _startTimeInvoke && stateInfo.normalizedTime < _stopTimeInvoke)
                    {
                        _isInvoked = true;
                        _visualEffectInvoker.Play(_visualEffectIndex);
                    }
                }
                else
                {
                    if (stateInfo.normalizedTime > _startTimeInvoke && stateInfo.normalizedTime > _stopTimeInvoke)
                    {
                        _isInvoked = true;
                        _visualEffectInvoker.Stop(_visualEffectIndex);
                    }
                }
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _isInvoked = false;
        }

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
    }
}
