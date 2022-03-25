using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBulletPowerUp
{
    //TODO: implement abstractbulletpowerup

    public abstract void applyPowerUp(AbstractAttackBehaviour bulletAttackBehaviour);
}

public class ExplodingBulletPowerUp : AbstractBulletPowerUp
{
    public ExplodingBulletPowerUp(){}

    public override void applyPowerUp(AbstractAttackBehaviour bulletAttackBehaviour)
    {
        bulletAttackBehaviour._hitTypeModifiers["explode"] += 1;
    }
}

public class PiercingBulletPowerUp : AbstractBulletPowerUp
{
    public PiercingBulletPowerUp(){Debug.Log("test");}

    public override void applyPowerUp(AbstractAttackBehaviour bulletAttackBehaviour)
    {
        bulletAttackBehaviour._hitTypeModifiers["pierce"] += 1;
    }
}
