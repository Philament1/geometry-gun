using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shapes
{
    public struct ShapeInfo
    {
        public string name, description;
        public bool isOnline;

        public ShapeInfo(string _name, string _description, bool _isOnline)
        {
            name = _name;
            description = _description;
            isOnline = _isOnline;
        }
    }

    public static readonly ShapeInfo[] shapes = new ShapeInfo[]
    {
        /*      COOL TEMPLATE BRO
        new ShapeInfo("Shape", 
            "Description", 
            false),
        */
        new ShapeInfo("Arrow",
            "<i>PRESS</i> to fire. If there is a bullet already travelling horizontally, it will be split into two vertically moving bullets instead.", 
            false),
        new ShapeInfo("Arrowhead",
            "<i>PRESS</i> to fire a bullet that slows and poisons. If the enemy is poisoned, the bullet will deal bonus damage and refresh the slow duration.", 
            true),
        new ShapeInfo("Castle", 
            "<i>PASSIVE</i> Castle continuously shoots bullets.\n<i>PRESS</i> to switch between two modes\n<i>Mobile mode</i>: Increased damage and movement speed, shoot one bullet at a time.\n<i>Tower mode</i>: Increased fire rate but slowed, heal while shooting, shoot three bullets at a time.", 
            false),
        new ShapeInfo("Chevron", 
            "<i>HOLD</i> to charge bullet.\n<i>RELEASE</i> to fire. Bullet deals more damage the more health the enemy has.\nCharging increases bullet speed.", 
            true),
        new ShapeInfo("Circle", 
            "<i>HOLD</i> to continuously fire.\n<i>RELEASE</i> to recharge.", 
            true),
        new ShapeInfo("Cloud",
            "<i>PASSIVE</i> Cloud spawns with a storm on the enemy side that will track, damage and temporarily freeze the enemy if they remain inside the storm. The storm's speed and damage will decay over time.\n<i>PRESS</i> to fire a bullet. If the bullet hits the storm, it will increase the storm's speed and damage.",
            true),
        new ShapeInfo("Crescent", 
            "<i>PASSIVE</i> Crescent gains fury over time. At maximum fury, gain increased fire rate, movement speed, armour and heal for every bullet fired. Fury lasts a few seconds and will reset to zero once over.\n<i>HOLD</i> to continuously fire two bullets. Hitting the enemy will increase fury.", 
            true),
        new ShapeInfo("Diamond", 
            "<i>HOLD</i> to charge, Diamond is slowed.\n<i>RELEASE</i> to fire homing bullets in burst that disintegrate quickly.\nCharging increases number of bullets.", 
            false),
        new ShapeInfo("Heart", 
            "<i>PASSIVE</i> Heart is invulnerable to the next damage they take if Heart has not taken damage for a short period of time.\n<i>HOLD</i> to charge.\n<i>RELEASE</i> to shoot a bullet that will destroy the first bullet, trap or turret it passes through.\nCharging increases size of bullet and damage.", 
            false),
        new ShapeInfo("Heptagon", 
            "<i>PRESS</i> to cast a Square spell.\n<i>HOLD</i> to cycle between spells, Heptagon is slowed.\n<i>RELEASE</i> to cast spell.\n<i>Square</i>: Fire a bullet that damages the enemy, deal bonus damage if enemy is frozen.\n<i>Circle</i>: Fire a bullet that freezes and damages enemy.\n<i>Triangle</i>: Shield Heptagon temporarily.", 
            false),
        new ShapeInfo("Hexagon", 
            "<i>PRESS</i> to fire a bullet.\n<i>HOLD</i> to charge a turret, Hexagon is stationary.\n<i>RELEASE</i> to spawn a turret is at maximum charge, shielding Hexagon. Turrets will continuously fire bullets then disintegrate.", 
            false),
        new ShapeInfo("Hourglass", 
            "<i>HOLD</i> to charge, Hourglass is slowed.\n<i>RELEASE</i> to shield Hourglass and fire bullets in burst.\nCharging to increases shield size and number of bullets.", 
            false),
        new ShapeInfo("Kite", 
            "<i>PRESS</i> to spawn a stationary bullet.\nEvery third <i>PRESS</i> will fire a bullet and mobilise all stationary bullets.", 
            false),
        new ShapeInfo("Lens", 
            "(Sprite not found)\n<i>PRESS</i> to fire a bullet that will remain stationary at the back wall. Every fourth <i>PRESS</i> will reverse all stationary bullets.", 
            false),
        new ShapeInfo("Lightning", 
            "<i>PASSIVE</i> Lightning is invulnerable and slowly drains health.\n<i>PRESS</i> to fire a bullet and heal Lightning. Lightning is vulnerable and slowed temporarily.", 
            false),
        new ShapeInfo("Octagon", 
            "<i>HOLD</i> to charge, Octagon is slowed.\n<i>RELEASE</i> to fire bullets that linger and slow the enemy when stationary.\nCharging increases bullet size, damage and linger time.", 
            false),
        new ShapeInfo("Oval", 
            "<i>HOLD</i> to continuously fire three bullets, Oval is slowed.\n<i>RELEASE</i> to recharge.", 
            false),
        new ShapeInfo("Parallelogram",
            "<i>HOLD</i> to charge.\n<i>RELEASE</i> to fire a bullet. If the bullet misses, it will remain stationary then expand vertically into a beam. The beam will slow and damage the enemy if they make contact.\nCharging will increase the bullet and beam width.",
            true),
        new ShapeInfo("Pentagon", 
            "<i>PASSIVE</i>Pentagon takes reduced damage.\n<i>PRESS</i> to fire a bullet that slows. If the enemy is slowed, the bullet will freeze and Pentagon will gain a shield. If the bullet is frozen, the bullet will refresh the freeze duration and increase Pentagon's shield size.", 
            true),
        new ShapeInfo("Plus", 
            "<i>PRESS</i> or <i>HOLD</i> to fire. Hitting the enemy will <i>Level up</i> Plus.\n<i>Level up</i>: Heal Plus and permanently increase Plus's fire rate, bullet speed, and movement speed. At max level, Plus gains armour", 
            true),
        new ShapeInfo("Rectangle", 
            "<i>PASSIVE</i> Rectangle spawns with reduced health.\n<i>PRESS</i> to fire bullets in burst, draining Rectangle's health. Heal Rectangle for every bullet hit.", 
            false),
        new ShapeInfo("Rocket",
            "<i>PRESS</i> or <i>HOLD</i> to fire. If the bullet hits, Rocket is given a temporary speed boost. If two consecutive bullets hit, the next bullet will be an <i>Empowered</i> bullet. All consecutive bullets hit will then be <i>Empowered</i> until a bullet misses.\n<i>Empowered</i> bullet: Deals bonus damage and heals Rocket if enemy is hit.",
            true),
        new ShapeInfo("Sector", 
            "(Sprite not found)\n<i>PASSIVE</i> Sector spawns with a missile launcher on the back wall.\n<i>PRESS</i> to fire a bullet, hitting the enemy will launch a homing missile from the missile launcher.", 
            false),
        new ShapeInfo("SemiCircle", 
            "<i>HOLD</i> to continuously fire five bullets and heal SemiCircle, SemiCircle is slowed.\n<i>RELEASE</i> to recharge.", 
            false),
        new ShapeInfo("Square",
            "<i>HOLD</i> to charge bullet, square is slowed.\n<i>RELEASE</i> to fire.\nCharging increases damage and bullet size.",
            true),
        new ShapeInfo("Star3", 
            "<i>PASSIVE</i> Star3 spawns with a target on the enemy map that moves with Star3.\n<i>PRESS</i> to spawn a bullet at the target location.", 
            false),
        new ShapeInfo("Star4", 
            "<i>PRESS</i> to fire a bullet that ricochets off the back wall.\nCatching the bullet when it returns will increase your charge.\nCharge increases bullet damage.", 
            false),
        new ShapeInfo("Star5", 
            "<i>PRESS</i> to fire a bullet.\n<i>HOLD</i> to charge an explosive.\n<i>RELEASE</i> to fire an explosive if at maximum charge.\nThe core of an explosive will slow the enemy. If the bullet hits am explosive, it will explode, dealing damage and slowing the enemy if in blast radius, and exploding any neaby explosives in blast radius.", 
            true),
        new ShapeInfo("Teardrop", 
            "<i>PASSIVE</i> Teardrop gains a shield after not taking damage for a short period of time.\n<i>PRESS</i> to fire an orb that increases in size the longer it is travelling for. When in contact with the enemy, the orb will slow down and encapsulate the enemy, dealing damage and slowing the enemy if they remain in the orb.", 
            false),
        new ShapeInfo("Trapezium", 
            "<i>HOLD</i> to charge, Trapezium is slowed.\n<i>RELEASE</i> to fire a horizontal beam that slows and damages the enemy.\nCharging increases beam height and linger time.", 
            false),
        new ShapeInfo("Triangle", 
            "<i>PRESS</i> to fire.\n<i>RELEASE</i> to increase number of bullets fired at once.", 
            true),
        new ShapeInfo("Xshape", 
            "<i>PRESS</i> to fire a bullet, dealing bonus damage if the enemy is trapped.\n<i>HOLD</i> to charge a trap, Xshape is slowed.\n<i>RELEASE</i> to fire a trap if at maximum charge. Traps will activate once stationary, slowing and poisoning the enemy if triggered.", 
            false),
    };


}
