using UnityEngine;

public class Anim_PlaySound : StateMachineBehaviour
{
    // -- FIELDS

    [SerializeField, Audio_] private string SoundName = default;
    [SerializeField] private bool PlayAtPosition = false;
    [SerializeField] private bool StopOnExit = false;
    [SerializeField] private bool SelfLoopingManagement = false;
    private AudioSource AudioSource = default;
    private bool IsPaused = false;

    // -- UNITY

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if( PlayAtPosition )
        {
            AudioSource = AudioController.S_PlaySound( SoundName, animator.transform.position );
        }
        else
        {
            AudioSource = AudioController.S_PlaySound( SoundName );
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if( !AudioSource )
        {
            return;
        }

        if( animator.speed == 0 )
        {
            if( !IsPaused )
            {
                AudioSource.Pause();
                IsPaused = true;
            }
            return;
        }
        else
        {
            if( IsPaused )
            {
                AudioSource.UnPause();
                IsPaused = false;
            }
        }

        if( SelfLoopingManagement )
        {
            if( !AudioSource.isPlaying )
            {
                AudioSource = AudioController.S_PlaySound( SoundName );
            }
        }

        if( PlayAtPosition )
        {
            AudioSource.transform.position = animator.transform.position;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if( StopOnExit )
        {
            AudioController.S_StopSound( SoundName );
            AudioSource.Stop();
        }
    }

    private void OnDisable()
    {
        if( StopOnExit && AudioSource )
        {
            AudioController.S_StopSound( SoundName );
            AudioSource.Stop();
        }
    }
}
