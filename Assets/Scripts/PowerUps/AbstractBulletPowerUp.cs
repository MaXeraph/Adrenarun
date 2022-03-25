using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBulletPowerUp
{
    //TODO: implement abstractbulletpowerup

    public abstract void applyPowerUp(AbstractAttackBehaviour bulletAttackBehaviour);
}

public class ExplosiveBulletPowerUp : AbstractBulletPowerUp
{
    public ExplosiveBulletPowerUp(){}

    public override void applyPowerUp(AbstractAttackBehaviour bulletAttackBehaviour)
    {
        Debug.Log("tried to apply");
        bulletAttackBehaviour._hitTypeModifiers["explode"] += 1;
        Debug.Log("applied explode");
    }
}

public class PiercingBulletPowerUp : AbstractBulletPowerUp
{
    public PiercingBulletPowerUp(){Debug.Log("test");}

    public override void applyPowerUp(AbstractAttackBehaviour bulletAttackBehaviour)
    {
        Debug.Log("tried to apply");
        bulletAttackBehaviour._hitTypeModifiers["pierce"] += 1;
        Debug.Log("applied pierce");
    }
}
