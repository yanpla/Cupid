using Cupid.Roles;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using UnityEngine;

namespace Cupid.Options;

public class CupidOptionsGroup : AbstractOptionGroup<CupidRole>
{
    public override string GroupName => "Cupid";
    public override Color GroupColor => Color.yellow;
    
    [ModdedNumberOption("Shoot Cooldown", min: 0, max: 60, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ShootCooldown { get; set; } = 10f;
}