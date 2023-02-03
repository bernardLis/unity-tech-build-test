using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/Database")]
public class GameDatabase : BaseScriptableObject
{
    public Sound _wrongSound;
    [SerializeField] Sound[] _correctSounds;
    public Sound GetRandomCorrectSound() { return _correctSounds[Random.Range(0, _correctSounds.Length)]; }


    [SerializeField] List<Sprite> AnimationSprites = new();
    public List<Sprite> GetAllAnimationSprites()
    {
        return AnimationSprites;
    }
}
