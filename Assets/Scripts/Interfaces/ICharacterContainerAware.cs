using UnityEngine;

/// <summary>
/// Monobehaviors which implement this interface will have SetParent called if they
/// are ultimately parented by a CharacterContainer. SetParent will be called during Spawn.
/// </summary>
public interface ICharacterContainerAware {

    void SetParent(GameObject parent);

}