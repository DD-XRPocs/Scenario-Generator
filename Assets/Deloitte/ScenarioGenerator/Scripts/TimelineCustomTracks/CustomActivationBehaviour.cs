using UnityEngine;
using UnityEngine.Playables;

public class CustomActivationBehaviour : PlayableBehaviour
{
    public GameObject target;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (target != null)
        {
            target.SetActive(true);
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (target != null)
        {
            target.SetActive(false);
        }
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        GameObject go = playerData as GameObject;

        if (go != null)
        {
            // Enable the GameObject when the clip starts playing and disable it when it stops.
            go.SetActive(info.weight > 0);
        }
    }
}
