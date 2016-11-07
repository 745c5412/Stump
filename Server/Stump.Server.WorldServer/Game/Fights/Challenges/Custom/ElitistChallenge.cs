﻿using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.ELITISTE)]
    public class ElitistChallenge : DefaultChallenge
    {
        public ElitistChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 50;
            BonusMax = 50;
        }

        public override void Initialize()
        {
            base.Initialize();

            Target = Fight.GetRandomFighter<MonsterFighter>();
            Target.Dead += OnDead;

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
            {
                fighter.BeforeDamageInflicted += OnBeforeDamageInflicted;
            }
        }

        public override bool IsEligible()
        {
            return Fight.GetAllFighters<MonsterFighter>().Count() > 1;
        }

        private void OnDead(FightActor fighter, FightActor killer)
        {
            Target.Dead -= OnDead;
            Target.BeforeDamageInflicted -= OnBeforeDamageInflicted;

            UpdateStatus(ChallengeStatusEnum.SUCCESS);
        }

        private void OnBeforeDamageInflicted(FightActor fighter, Damage damage)
        {
            if (!(damage.Source is CharacterFighter))
                return;

            if (Target != fighter)
                UpdateStatus(ChallengeStatusEnum.FAILED, damage.Source);
        }
    }
}