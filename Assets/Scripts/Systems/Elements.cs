using UnityEngine;

public enum ElementType { Grass, Fire, Water }

public static class ElementRules
{
// Returns a multiplier depending on the attack and defender
public static float DamageMultiplier(ElementType attacker, ElementType defender)
{
// Grass > Water, Water > Fire, Fire > Grass
if (attacker == ElementType.Grass && defender == ElementType.Water) return 1.5f;
if (attacker == ElementType.Water && defender == ElementType.Fire) return 1.5f;
if (attacker == ElementType.Fire && defender == ElementType.Grass) return 1.5f;

// Weaknesses
if (defender == ElementType.Grass && attacker == ElementType.Water) return 0.5f;
if (defender == ElementType.Water && attacker == ElementType.Grass) return 0.5f;
if (defender == ElementType.Fire && attacker == ElementType.Water) return 0.5f;

return 1f;
}
}