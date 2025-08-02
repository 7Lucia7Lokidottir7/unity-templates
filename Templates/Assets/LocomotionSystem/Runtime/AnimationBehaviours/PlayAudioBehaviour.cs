using UnityEngine;
using UnityEngine.Audio;
namespace PG.LocomotionSystem
{
    public class PlayAudioBehaviour : StateMachineBehaviour
    {
        [SerializeField] private string _audioName;
        [SerializeField] private AudioResource _audioResource;
        private AudioSource _audioSource;
        [SerializeField, Range(0, 1)] private float _startTimePlay;
        [SerializeField, Range(0, 1)] private float _stopTimePlay;
        [SerializeField] private float _destroyTime = 2f;
        [SerializeField] private bool _isInvokeOnce;
        private bool _isInvoked;
        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AudioSource[] audioSources = animator.GetComponentsInChildren<AudioSource>();
            for (int i = 0; i < audioSources.Length; i++)
            {
                if (audioSources[i].name == _audioName)
                {
                    _audioSource = audioSources[i];
                    break;
                }
            }
        }

        // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_isInvokeOnce)
            {
                if (!_isInvoked)
                {
                    if (stateInfo.normalizedTime > _startTimePlay)
                    {
                        _isInvoked = true;
                        AudioSource audioSource = Instantiate(_audioSource, animator.transform);
                        audioSource.resource = _audioResource;
                        audioSource.Play();
                        Destroy(audioSource.gameObject, _destroyTime);
                    }
                }
            }
            else
            {
                if (!_isInvoked)
                {
                    if (stateInfo.normalizedTime > _startTimePlay && stateInfo.normalizedTime < _stopTimePlay)
                    {
                        _isInvoked = true;
                        _audioSource.resource = _audioResource;
                        _audioSource.Play();
                    }
                }
                else
                {
                    if (stateInfo.normalizedTime > _startTimePlay && stateInfo.normalizedTime > _stopTimePlay)
                    {
                        _isInvoked = true;
                        _audioSource.Stop();
                    }
                }
            }
        }

        // OnStateExit is called before OnStateExit is called on any state inside this state machine
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _isInvoked = false;
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
        //    
        //}

        // OnStateMachineExit is called when exiting a state machine via its Exit Node
        //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        //{
        //    
        //}
    }
}