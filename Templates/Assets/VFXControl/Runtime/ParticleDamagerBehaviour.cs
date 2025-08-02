using UnityEngine;
namespace PG.VFXControl
{
    public class ParticleDamagerBehaviour : StateMachineBehaviour
    {
        private ParticleInvoker _particleInvoker;
        [SerializeField, Range(0, 1)] private float _startTimeInvoke = 0.25f;
        [SerializeField, Range(0, 1)] private float _stopTimeInvoke = 0.75f;
        [SerializeField] private int _particleIndex;
        private Collider _collider;
        private bool _hasActivated;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _particleInvoker = animator.GetComponentInChildren<ParticleInvoker>();
            _collider = _particleInvoker.GetParticleSystem(_particleIndex).GetComponent<Collider>();
            _collider.enabled = false;
            _hasActivated = false;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            bool inWindow = stateInfo.normalizedTime >= _startTimeInvoke
                         && stateInfo.normalizedTime < _stopTimeInvoke;

            if (inWindow && !_hasActivated)
            {
                _collider.enabled = true;
                _hasActivated = true;
            }
            else if (!inWindow && _hasActivated)
            {
                _collider.enabled = false;
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _collider.enabled = false;
            _hasActivated = false;
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
