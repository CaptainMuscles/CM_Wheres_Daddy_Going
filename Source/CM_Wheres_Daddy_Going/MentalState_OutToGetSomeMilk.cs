using System.Collections.Generic;
using System.Linq;

using RimWorld;
using Verse;
using Verse.AI;

namespace CM_Wheres_Daddy_Going
{
    public class MentalState_OutToGetSomeMilk : MentalState
    {
        private Pawn child = null;
        private ThingDef thingDef = null;

        public override string InspectLine => string.Format(base.InspectLine, thingDef.label);

        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }

        public override void PreStart()
        {
            base.PreStart();

            ParentalMemory memory = WheresDaddyGoingMod.Instance.MemoriesOfDad.GetOrCreateMemory(pawn);

            List<Pawn> childrenOnMap = pawn.relations.Children.Where(child => child.Map == pawn.Map).ToList();
            if (childrenOnMap.Count > 0)
                child = childrenOnMap.RandomElement();

            thingDef = memory.thingDef;
            if (thingDef == null)
            {
                thingDef = DefDatabase<ThingDef>.GetNamed("Milk");
                
                if (thingDef == null)
                    thingDef = ThingSetMaker_ResourcePod.RandomPodContentsDef(false);

                memory.thingDef = thingDef;
            }
        }

        public override string GetBeginLetterText()
        {
            string baseLetter = "";
            if (child == null || pawn.gender == Gender.None)
            {
                baseLetter = def.beginLetter.Formatted(pawn.NameShortColored, thingDef.label, pawn.Named("PAWN")).AdjustedFor(pawn).Resolve().CapitalizeFirst();
            }
            else if (pawn.gender == Gender.Male)
            {
                baseLetter = "CM_Wheres_Daddy_Going_MentalState_OutToGetSomeMilk_Letter_Daddy".Translate(thingDef.label, child.NameShortColored);
            }
            else if (pawn.gender == Gender.Female)
            {
                baseLetter = "CM_Wheres_Daddy_Going_MentalState_OutToGetSomeMilk_Letter_Mommy".Translate(thingDef.label, child.NameShortColored);
            }

            return baseLetter;
            //return def.beginLetter.Formatted(pawn.NameShortColored, thingDef.label, pawn.Named("PAWN")).AdjustedFor(pawn).Resolve().CapitalizeFirst();
            //return def.beginLetter.Formatted(pawn.NameShortColored, thingDef.label, pawn.Named("PAWN"), thingDef.Named("TARGET")).AdjustedFor(pawn).Resolve().CapitalizeFirst();
            //return def.beginLetter.Formatted(pawn.NameShortColored, target.NameShortColored, pawn.Named("PAWN"), target.Named("TARGET")).AdjustedFor(pawn).Resolve().CapitalizeFirst();
        }
    }
}