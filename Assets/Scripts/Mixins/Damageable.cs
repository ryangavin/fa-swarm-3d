using UnityEngine;

public class Damageable : MonoBehaviour {

    public void Damage(GameObject source, int damage) {
        DamageEvent eventArgs = new DamageEvent(source, gameObject, damage);
        EventBus.Publish(eventArgs);
    }

}
