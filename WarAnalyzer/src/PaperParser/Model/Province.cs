using System.Collections.Generic;
using System.Linq;
using PaperParser.Actions;

namespace PaperParser.Model
{
    public class Province
    {
        public Province(string provinceName)
        {
            ProvinceName = provinceName;
            AllActions = new List<UtopiaAction>();
        }

        public string ProvinceName { get; set; }
        public int AidSent { get; set; }
        public int AidReceived { get; set; }
        public int AmbushLost { get; set; }
        public int LandLost { get; set; }
        public int AmbushReceived { get; set; }
        public int AttacksReceived { get; set; }
        public int AttacksMade { get; set; }
        public int AmbushMade { get; set; }
        public int AmbushGained { get; set; }
        public int LandGained { get; set; }
        public int ConquestMade { get; set; }
        public int ConquestGained { get; set; }
        public int TraditionalMarchGained { get; set; }
        public int TraditionalMarchMade { get; set; }
        public int TraditionalMarchReceived { get; set; }
        public int TraditionalMarchLost { get; set; }
        public int RazeMade { get; set; }
        public int LandRazed { get; set; }
        public int RazeReceived { get; set; }
        public int LandLostRazed { get; set; }
        public int MassacreMade { get; set; }
        public int PeopleKilled { get; set; }
        public int MassacreReceived { get; set; }
        public int PeopleLost { get; set; }
        public int PlunderMade { get; set; }
        public int PlunderReceived { get; set; }
        public int LearnMade { get; set; }
        public int LearnReceived { get; set; }
        public int DragonsSlain { get; set; }
        public int FailedAttacksMade { get; set; }
        public int FailedAttacksReceived { get; set; }

        public List<UtopiaAction> AllActions { get; }

        public string GetFirstAttack()
        {
            var allAttacks = AllActions.OfType<AttackMadeAction>().ToList();
            if (allAttacks.Any())
            {
                return allAttacks.OrderBy(a => a.Date.GetDayNumber()).First().Date.ToString();
            }
            return "N/A";
        }

        public string GetLastAttack()
        {
            var allAttacks = AllActions.OfType<AttackMadeAction>().ToList();
            if (allAttacks.Any())
            {
                return allAttacks.OrderByDescending(a => a.Date.GetDayNumber()).First().Date.ToString();
            }
            return "N/A";
        }

        public double TimeBetweenUniques()
        {
            var uniques = NumberOfUniques();
            
            if (uniques > 1)
            {
                var allAttacks = AllActions.OfType<AttackMadeAction>().ToList();
                var firstDayInt = allAttacks.Min(a => a.Date.GetDayNumber());
                var lastDayInt = allAttacks.Max(a => a.Date.GetDayNumber());

                return ((1.00 * lastDayInt - firstDayInt)/(uniques - 1));
            }
            return double.MaxValue;
        }

        public string TimeBetweenUniquesString()
        {
            var timeBetweenUniques = TimeBetweenUniques();

            if (timeBetweenUniques == double.MaxValue)
            {
                return "N/A";
            }

            return timeBetweenUniques.ToString("F2") + " houres";
        }

        public int NumberOfUniques()
        {
            var allAttacks = AllActions.OfType<AttackMadeAction>().ToList();

            if (allAttacks.Any())
            {
                var uniques = 0;
                var attacks = allAttacks.Select(a => a).ToList();
                var minDayInt = attacks.Min(a => a.Date.GetDayNumber());

                while (attacks.Any())
                {
                    uniques++;
                    attacks = attacks.Where(a => a.Date.GetDayNumber() > minDayInt + 4).ToList();

                    if (attacks.Any())
                    {
                        minDayInt = attacks.Min(a => a.Date.GetDayNumber());
                    }
                }

                return uniques;
            }

            return 0;
        }

        public string AverageUniqueGain()
        {
            var uniques = NumberOfUniques();

            if (uniques > 0)
            {
                return (LandGained/uniques).ToString("F0");
            }

            return "0";
        }

        public void AddAidSent(AidAction action)
        {
            AidSent++;
            AllActions.Add(action);
        }

        public void AddAidReceived(AidAction action)
        {
            AidReceived++;
            AllActions.Add(action);
        }

        public void AddAmbushReceived(AmbushReceivedAction action)
        {
            LandLost += action.Land;
            AmbushLost += action.Land;
            AmbushReceived++;
            AttacksReceived++;
            AllActions.Add(action);
        }

        public void AddAmbushMade(AmbushMadeAction action)
        {
            LandGained += action.Land;
            AmbushGained += action.Land;
            AmbushMade++;
            AttacksMade++;
            AllActions.Add(action);
        }

        public void AddConquestMade(ConquestMadeAction action)
        {
            LandGained += action.Land;
            ConquestGained += action.Land;
            ConquestMade++;
            AttacksMade++;
            AllActions.Add(action);
        }

        public void AddTraditionalMarchMade(TraditionalMarchMadeAction action)
        {
            LandGained += action.Land;
            TraditionalMarchGained += action.Land;
            TraditionalMarchMade++;
            AttacksMade++;
            AllActions.Add(action);
        }

        public void AddTraditionalMarchReceived(TraditionalMarchReceivedAction action)
        {
            LandLost += action.Land;
            TraditionalMarchLost += action.Land;
            TraditionalMarchReceived++;
            AttacksReceived++;
            AllActions.Add(action);
        }

        public void AddRazeMade(RazeMadeAction action)
        {
            LandRazed += action.Buildings;
            RazeMade++;
            AttacksMade++;
            AllActions.Add(action);
        }

        public void AddRazeReceived(RazeReceivedAction action)
        {
            LandLostRazed += action.Buildings;
            RazeReceived++;
            AttacksReceived++;
            AllActions.Add(action);
        }

        public void AddMassacreMade(MassacreMadeAction action)
        {
            PeopleKilled += action.People;
            MassacreMade++;
            AttacksMade++;
            AllActions.Add(action);
        }

        public void AddMassacreReceived(MassacreReceivedAction action)
        {
            PeopleLost += action.People;
            MassacreReceived++;
            AttacksReceived++;
            AllActions.Add(action);
        }

        public void AddPlunderMade(PlunderMadeAction action)
        {
            PlunderMade++;
            AttacksMade++;
            AllActions.Add(action);
        }

        public void AddPlunderReceived(PlunderReceivedAction action)
        {
            PlunderReceived++;
            AttacksReceived++;
            AllActions.Add(action);
        }

        public void AddLearnMade(LearnMadeAction action)
        {
            LearnMade++;
            AttacksMade++;
            AllActions.Add(action);
        }

        public void AddLearnReceived(LearnReceivedAction action)
        {
            LearnReceived++;
            AttacksReceived++;
            AllActions.Add(action);
        }

        public void AddDragonSlay(DragonSlayAction action)
        {
            DragonsSlain++;
            AllActions.Add(action);
        }

        public void AddFailedAttackMade(FailedAttackMadeAction action)
        {
            FailedAttacksMade++;
            AttacksMade++;
            AllActions.Add(action);
        }

        public void AddFailedAttackReceived(FailedAttackReceivedAction action)
        {
            FailedAttacksReceived++;
            AttacksReceived++;
            AllActions.Add(action);
        }

        public int HeaviestChain()
        {
            var allReceived = AllActions.OfType<LandAttackReceivedAction>().ToList();

            if (allReceived.Any())
            {
                var heaviest = 0;

                foreach (var received in allReceived)
                {
                    var sum =
                        allReceived.Where(a => a.Date.GetDayNumber() >= received.Date.GetDayNumber())
                            .Where(a => a.Date.GetDayNumber() < received.Date.GetDayNumber() + 5)
                            .Sum(a => a.Land);

                    if (sum > heaviest)
                    {
                        heaviest = sum;
                    }
                }

                return heaviest;
            }

            return 0;
        }

        public int NetGain()
        {
            return LandGained - LandLost;
        }
    }
}