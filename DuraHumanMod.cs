using UnityEngine;

namespace DuraHuman
{
    public class DuraHuman
    {
		//allows us to modify attributes.
		public Modification humanObject = null;
		
		//inits human
        public DuraHuman(float durabilityVal, float damageMultiplier, bool hasStrongLimbs, bool skipAfterSpawn)
        {
			humanObject = new Modification();
			humanObject.OriginalItem = ModAPI.FindSpawnable("Human");
			humanObject.NameOverride = "Human (" + durabilityVal + "x)";
			humanObject.DescriptionOverride = "A human with " + durabilityVal + "x times the original durability." + (hasStrongLimbs ? " This human has tougher limbs." : "");
			humanObject.CategoryOverride = ModAPI.FindCategory("Entities");
			if (!skipAfterSpawn)
			{
				humanObject.AfterSpawn = delegate(GameObject Instance)
				{
					//we need this method so users can make custom skins or add other attributes.
					AssignDurabilityToLimbs(Instance, durabilityVal, damageMultiplier, hasStrongLimbs);
				};
			}
        }
		
		//assigns durability attributes to all limbs.
		public void AssignDurabilityToLimbs(GameObject Instance, float durabilityVal, float damageMultiplier, bool hasStrongLimbs)
		{
			var person = Instance.GetComponent<PersonBehaviour>();
			foreach (LimbBehaviour limb in person.Limbs)
			{
				limb.Health *= durabilityVal;
				limb.BreakingThreshold *= durabilityVal;
				//limb.BalanceMuscleMovement *= durabilityVal;
				//limb.IsAndroid = hasStrongLimbs;
				limb.ImmuneToDamage = hasStrongLimbs; 
				limb.ImpactPainMultiplier *= damageMultiplier;
				limb.ShotDamageMultiplier *= damageMultiplier;
				//limb.DoBalanceJerk = !hasStrongLimbs;
			}
		}
    }
	
	public class DuraHumanMod
	{
		//creates all the entries
		public static void Main()
        {
			//HACK because using a for loop didn't work.
			CreateDuraHuman(2f, 0.8f, false);
			CreateDuraHuman(4f, 0.5f, false);
			CreateDuraHuman(10f, 0.1f, false);
			CreateDuraHuman(50f, 0.05f, true);
			CreateDuraHuman(100f, 0.01f, true);
			CreateDuraHuman(1000f, 0.005f, true);
			CreateDuraHuman(1000000f, 0.001f, true);
        }
		
		//creates a basic DuraHuman with NO custom skin or custom attributes.
		public static void CreateDuraHuman(float durabilityVal, float damageMultiplier, bool hasStrongLimbs)
		{
			DuraHuman human = new DuraHuman(durabilityVal, damageMultiplier, hasStrongLimbs, false);
			ModAPI.Register(human.humanObject);
		}
	}
}