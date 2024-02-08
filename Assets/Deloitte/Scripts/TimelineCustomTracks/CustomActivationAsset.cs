using UnityEngine.Playables;
using UnityEngine;

[System.Serializable]
public class CustomActivationAsset : PlayableAsset
{
    public ExposedReference<GameObject> targetObject;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CustomActivationBehaviour>.Create(graph);

        var behaviour = playable.GetBehaviour();
        behaviour.target = targetObject.Resolve(graph.GetResolver());

        return playable;
    }
}