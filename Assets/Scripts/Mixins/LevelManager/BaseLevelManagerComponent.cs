using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base component that all level manager components should extend from.
/// This class is basically empty, it just forces level manager components to be attached to a game object
/// which is a level manager.
/// </summary>
[RequireComponent(typeof(LevelManager))]
public class BaseLevelManagerComponent : MonoBehaviour {
}