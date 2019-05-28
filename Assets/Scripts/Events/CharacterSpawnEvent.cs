using UnityEngine;

public class CharacterSpawnEvent : Event {

    public readonly CharacterData CharacterData;

    public CharacterSpawnEvent(GameObject source, GameObject target, CharacterData characterData) : base(source, target) {
        CharacterData = characterData;
    }
}