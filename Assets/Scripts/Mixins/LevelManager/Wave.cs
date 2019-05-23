using System;
using System.Collections.Generic;

/// <summary>
/// Simple implementation of a wave. Also the base implementation that all fancier waves should extend.
///
/// This implementation takes a set of enemy spawners, and a set of characters to treat as enemies.
/// Enemies are spawned at random enemy spawners while trying to maintain the 
/// </summary>
public class Wave : BaseLevelManagerComponent {

    public List<EnemySpawner> enemySpanwers;
    public List<CharacterData> characterPool;

    private void Start() {
        throw new NotImplementedException();
    }
}