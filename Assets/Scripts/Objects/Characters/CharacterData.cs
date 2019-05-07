using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class CharacterData : ScriptableObject {
   
   public GameObject characterModel;
   public Weapon defaultWeapon;
   public int health;
   public float moveSpeed;
   public Shader deathShader;

}
