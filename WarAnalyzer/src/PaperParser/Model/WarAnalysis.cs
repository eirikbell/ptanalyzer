using System;
using System.Collections.Generic;
using System.Linq;
using PaperParser.Actions;

namespace PaperParser.Model
{
    public class WarAnalysis
    {
        public WarAnalysis()
        {
            Provinces = new List<Province>();
            UnknownActions = new List<UnknownAction>();
            CeaseFireActions = new List<CeaseFireAction>();
            WarActions = new List<UtopiaAction>();
            DragonActions = new List<DragonAction>();
            KdManagementActions = new List<KdManagementAction>();

            DisplayAdvanced = false;
        }

        public List<UnknownAction> UnknownActions { get; }
        public List<CeaseFireAction> CeaseFireActions { get; set; }
        public List<UtopiaAction> WarActions { get; set; }
        public List<DragonAction> DragonActions { get; set; }
        public List<KdManagementAction> KdManagementActions { get; set; }
        public List<Province> Provinces { get; }
        public UtopiaDate WarStart { get; set; }
        public bool DisplayAdvanced { get; set; }

        public void AddAction(string action)
        {
            var utoDate = GetUtoDate(action);

            var actionString = action.Replace(utoDate.ToString(), "").Trim();
            var actionType = GetActionType(actionString);
            
            switch (actionType)
            {
                case ActionType.Unknown:
                    GenericAction(utoDate, actionString);
                    break;
                case ActionType.Aid:
                    AidAction(utoDate, actionString);
                    break;
                case ActionType.TraditionalMarch:
                    TraditionalMarchAction(utoDate, actionString);
                    break;
                case ActionType.Conquest:
                    ConquestAction(utoDate, actionString);
                    break;
                case ActionType.Ambush:
                    AmbushAction(utoDate, actionString);
                    break;
                case ActionType.Raze:
                    RazeAction(utoDate, actionString);
                    break;
                case ActionType.Plunder:
                    PlunderAction(utoDate, actionString);
                    break;
                case ActionType.Learn:
                    LearnAction(utoDate, actionString);
                    break;
                case ActionType.Massacre:
                    MassacreAction(utoDate, actionString);
                    break;
                case ActionType.DragonSlay:
                    DragonSlayAction(utoDate, actionString);
                    break;
                case ActionType.Failed:
                    FailedAction(utoDate, actionString);
                    break;
                case ActionType.CeaseFire:
                    CeaseFireAction(utoDate, actionString);
                    break;
                case ActionType.WarDeclare:
                    WarDeclareAction(utoDate, actionString);
                    break;
                case ActionType.WarEnd:
                    WarEndAction(utoDate, actionString);
                    break;
                case ActionType.Dragon:
                    DragonAction(utoDate, actionString);
                    break;
                case ActionType.KdManagement:
                    KdManagementAction(utoDate, actionString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void KdManagementAction(UtopiaDate utoDate, string actionString)
        {
            var action = new KdManagementAction
            {
                Date = utoDate,
                ActionString = actionString
            };

            KdManagementActions.Add(action);
        }

        private void DragonAction(UtopiaDate utoDate, string actionString)
        {
            var action = new DragonAction
            {
                Date = utoDate,
                ActionString = actionString
            };

            DragonActions.Add(action);
        }

        private void CeaseFireAction(UtopiaDate utoDate, string actionString)
        {
            var action = new CeaseFireAction
            {
                Date = utoDate,
                ActionString = actionString
            };

            CeaseFireActions.Add(action);
        }

        private void WarDeclareAction(UtopiaDate utoDate, string actionString)
        {
            if (actionString.Contains("declared WAR"))
            {
                WarStart = utoDate;
            }

            var action = new WarDeclareAction
            {
                Date = utoDate,
                ActionString = actionString
            };

            WarActions.Add(action);
        }

        private void WarEndAction(UtopiaDate utoDate, string actionString)
        {
            var action = new WarDeclareAction
            {
                Date = utoDate,
                ActionString = actionString
            };

            WarActions.Add(action);
        }

        private void LearnAction(UtopiaDate utoDate, string actionString)
        {
            if (actionString.Contains("invaded and stole"))
            {
                // January 21 of YR3 Hideki Tiger Shark (6:18) invaded and stole from Maceio (9:19).

                var endNameIndex = actionString.IndexOf("(") - 1;
                var provinceName = actionString.Substring(0, endNameIndex).Trim();

                var action = new LearnMadeAction
                {
                    Date = utoDate,
                    ActionString = actionString
                };

                var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
                if (province == null)
                {
                    province = new Province(provinceName);
                    Provinces.Add(province);
                }
                province.AddLearnMade(action);
            }
            else
            {
                // February 1 of YR3 MeLikeBiiigBoooooobs (5:20) attacked and stole from Joseph Stallion (6:18).

                var startNameIndex = actionString.IndexOf("attacked and stole from ") + "attacked and stole from ".Length;
                var nameTempString = actionString.Substring(startNameIndex);
                var endNameIndex = nameTempString.IndexOf("(") - 1;
                var provinceName = nameTempString.Substring(0, endNameIndex).Trim();

                var action = new LearnReceivedAction
                {
                    Date = utoDate,
                    ActionString = actionString
                };

                var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
                if (province == null)
                {
                    province = new Province(provinceName);
                    Provinces.Add(province);
                }
                province.AddLearnReceived(action);
            }
        }

        private void PlunderAction(UtopiaDate utoDate, string actionString)
        {
            if (actionString.Contains("invaded and pillaged"))
            {
                // February 2 of YR0 An unknown province from Dicktators (6:18) invaded and pillaged Dadria (3:18).

                var endNameIndex = actionString.IndexOf("(") - 1;
                var provinceName = actionString.Substring(0, endNameIndex).Trim();

                var action = new PlunderMadeAction
                {
                    Date = utoDate,
                    ActionString = actionString
                };

                var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
                if (province == null)
                {
                    province = new Province(provinceName);
                    Provinces.Add(province);
                }
                province.AddPlunderMade(action);
            }
            else
            {
                var startNameIndex = actionString.IndexOf("pillaged the lands of ") + "pillaged the lands of ".Length;
                var nameTempString = actionString.Substring(startNameIndex);
                var endNameIndex = nameTempString.IndexOf("(") - 1;
                var provinceName = nameTempString.Substring(0, endNameIndex).Trim();

                var action = new PlunderReceivedAction
                {
                    Date = utoDate,
                    ActionString = actionString
                };

                var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
                if (province == null)
                {
                    province = new Province(provinceName);
                    Provinces.Add(province);
                }
                province.AddPlunderReceived(action);
            }
        }

        private void FailedAction(UtopiaDate utoDate, string actionString)
        {
            // June 20 of YR1 Genghis Keel Billed Toucan (6:18) attempted an invasion of Manbearpig (5:7), but was repelled.

            if (actionString.Contains("repelled."))
            {
                var endNameIndex = actionString.IndexOf("(");
                var provinceName = actionString.Substring(0, endNameIndex).Trim();

                var action = new FailedAttackMadeAction
                {
                    Date = utoDate,
                    ActionString = actionString
                };
                var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
                if (province == null)
                {
                    province = new Province(provinceName);
                    Provinces.Add(province);
                }
                province.AddFailedAttackMade(action);
            }
            else
            {
                // April 20 of YR1 Wait nvmd Dont Hit Tech (5:7) attempted to invade Indri Amin (6:18).
                var sIndex = actionString.IndexOf("attempted to invade") + "attempted to invade".Length;
                
                var tempString = actionString.Substring(sIndex).Trim();

                var endNameIndex = tempString.IndexOf("(") - 1;
                var provinceName = tempString.Substring(0, endNameIndex).Trim();
                
                var action = new FailedAttackReceivedAction
                {
                    Date = utoDate,
                    ActionString = actionString
                };
                var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
                if (province == null)
                {
                    province = new Province(provinceName);
                    Provinces.Add(province);
                }
                province.AddFailedAttackReceived(action);
            }
        }

        private void DragonSlayAction(UtopiaDate utoDate, string actionString)
        {
            var endNameIndex = actionString.IndexOf("has slain");
            var provinceName = actionString.Substring(0, endNameIndex).Trim();

            var action = new DragonSlayAction
            {
                Date = utoDate,
                ActionString = actionString
            };
            var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
            if (province == null)
            {
                province = new Province(provinceName);
                Provinces.Add(province);
            }
            province.AddDragonSlay(action);

            DragonAction(utoDate, actionString);
        }

        private void MassacreAction(UtopiaDate utoDate, string actionString)
        {
            if (actionString.Contains("people within"))
            {
                //May 17 of YR1 Adolf Hippo (6:18) killed 598 people within Icecream (5:7).

                var endNameIndex = actionString.IndexOf("(") - 1;
                var provinceName = actionString.Substring(0, endNameIndex).Trim();

                var startPeopleIndex = actionString.IndexOf(" killed ") + " killed ".Length;
                var peopleTempString = actionString.Substring(startPeopleIndex);

                var splitted = peopleTempString.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                var peopleString = splitted[0].Replace(",", "");
                int people = int.Parse(peopleString);

                var action = new MassacreMadeAction
                {
                    Date = utoDate,
                    ActionString = actionString,
                    People = people
                };
                var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
                if (province == null)
                {
                    province = new Province(provinceName);
                    Provinces.Add(province);
                }
                province.AddMassacreMade(action);
            }
            else
            {
                //June 11 of YR1 State of Confusion (5:7) invaded Palpatine Sardine (6:18) and killed 53 people.

                var startNameIndex = actionString.IndexOf(" invaded ") + " invaded ".Length;
                var nameTempString = actionString.Substring(startNameIndex);
                var endNameIndex = nameTempString.IndexOf("(") - 1;
                var provinceName = nameTempString.Substring(0, endNameIndex).Trim();

                var startLandIndex = actionString.IndexOf(" killed ") + " killed ".Length;
                var peopleTempString = actionString.Substring(startLandIndex);

                var splitted = peopleTempString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                int people = int.Parse(splitted[0]);

                var action = new MassacreReceivedAction
                {
                    Date = utoDate,
                    ActionString = actionString,
                    People = people
                };
                var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
                if (province == null)
                {
                    province = new Province(provinceName);
                    Provinces.Add(province);
                }
                province.AddMassacreReceived(action);
            }
        }

        private void RazeAction(UtopiaDate utoDate, string actionString)
        {

            // June 5 of YR1 Benito MOTHolini (6:18) invaded Icecream (5:7) and razed 51 acres of land.

            if (actionString.Contains("(6:18) invaded"))
            {
                var endNameIndex = actionString.IndexOf("(") - 1;
                var provinceName = actionString.Substring(0, endNameIndex).Trim();

                var startLandIndex = actionString.IndexOf(" razed ") + " razed ".Length;
                var landTempString = actionString.Substring(startLandIndex);

                var splitted = landTempString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                int land = int.Parse(splitted[0]);

                var action = new RazeMadeAction
                {
                    Date = utoDate,
                    ActionString = actionString,
                    Buildings = land
                };
                var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
                if (province == null)
                {
                    province = new Province(provinceName);
                    Provinces.Add(province);
                }
                province.AddRazeMade(action);
            }
            else
            {
                // April 22 of YR1 oldsword (5:7) razed 47 acres of Kim Jong UNicorn (6:18).
                var startNameIndex = actionString.IndexOf(" acres of ") + " acres of ".Length;
                var nameTempString = actionString.Substring(startNameIndex);
                var endNameIndex = nameTempString.IndexOf("(") - 1;
                var provinceName = nameTempString.Substring(0, endNameIndex).Trim();

                var startLandIndex = actionString.IndexOf(" razed ") + " razed ".Length;
                var landTempString = actionString.Substring(startLandIndex);

                var splitted = landTempString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                int land = int.Parse(splitted[0]);

                var action = new RazeReceivedAction
                {
                    Date = utoDate,
                    ActionString = actionString,
                    Buildings = land
                };
                var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
                if (province == null)
                {
                    province = new Province(provinceName);
                    Provinces.Add(province);
                }
                province.AddRazeReceived(action);
            }
        }

        private void TraditionalMarchAction(UtopiaDate utoDate, string actionString)
        {
            if (actionString.Contains(") invaded "))
            {
                var sIndex = actionString.IndexOf(") invaded ") + ") invaded ".Length;

                //April 20 of YR1 Norths Flying Circus (5:7) invaded Vlad The Impala (6:18) and captured 51 acres of land.
                var tempString = actionString.Substring(sIndex).Trim();

                var endNameIndex = tempString.IndexOf("(") - 1;
                var provinceName = tempString.Substring(0, endNameIndex).Trim();

                var startLandIndex = tempString.IndexOf(" and captured ") + " and captured ".Length;
                var landTempString = tempString.Substring(startLandIndex);

                var splitted = landTempString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                int land = int.Parse(splitted[0]);

                var action = new TraditionalMarchReceivedAction
                {
                    Date = utoDate,
                    ActionString = actionString,
                    Land = land
                };
                var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
                if (province == null)
                {
                    province = new Province(provinceName);
                    Provinces.Add(province);
                }
                province.AddTraditionalMarchReceived(action);
            }
            else
            {
                // April 21 of YR1 Ying Zheng (6:18), captured 21 acres of land from Here comes the Boom (5:7).

                var endNameIndex = actionString.IndexOf("(") - 1;
                var provinceName = actionString.Substring(0, endNameIndex).Trim();

                var startLandIndex = actionString.IndexOf(" captured") + " captured".Length;
                var landTempString = actionString.Substring(startLandIndex);

                var splitted = landTempString.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                int land = int.Parse(splitted[0]);

                var action = new TraditionalMarchMadeAction
                {
                    Date = utoDate,
                    ActionString = actionString,
                    Land = land
                };
                var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
                if (province == null)
                {
                    province = new Province(provinceName);
                    Provinces.Add(province);
                }
                province.AddTraditionalMarchMade(action);
            }
        }

        private void ConquestAction(UtopiaDate utoDate, string actionString)
        {
            // April 21 of YR1 Ying Zheng (6:18), captured 21 acres of land from Here comes the Boom (5:7).

            var endNameIndex = actionString.IndexOf("(") - 1;
            var provinceName = actionString.Substring(0, endNameIndex).Trim();

            var startLandIndex = actionString.IndexOf(", captured") + ", captured".Length;
            var landTempString = actionString.Substring(startLandIndex);

            var splitted = landTempString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            int land = int.Parse(splitted[0]);

            var action = new ConquestMadeAction
            {
                Date = utoDate,
                ActionString = actionString,
                Land = land
            }; var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
            if (province == null)
            {
                province = new Province(provinceName);
                Provinces.Add(province);
            }
            province.AddConquestMade(action);
        }

        private void AmbushAction(UtopiaDate utoDate, string actionString)
        {
            var armiesFromText = " ambushed armies from ";
            if (actionString.Contains(armiesFromText))
            {
                //received

                var startIndex = actionString.IndexOf(armiesFromText) + armiesFromText.Length;
                var tempString = actionString.Substring(startIndex);

                var endNameIndex = tempString.IndexOf("(") - 1;
                var provinceName = tempString.Substring(0, endNameIndex).Trim();

                var startLandIndex = tempString.IndexOf(" and took") + " and took".Length;
                var landTempString = tempString.Substring(startLandIndex);

                var splitted = landTempString.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                int land = int.Parse(splitted[0]);

                var action = new AmbushReceivedAction
                {
                    Date = utoDate,
                    ActionString = actionString,
                    Land = land
                };

                var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
                if (province == null)
                {
                    province = new Province(provinceName);
                    Provinces.Add(province);
                }
                province.AddAmbushReceived(action);
            }
            else
            {
                //  May 23 of YR1 An unknown province from Dicktators Going to WARRRR (6:18) recaptured 40 acres of land from N3k0rb (5:7). 

                // made

                var endNameIndex = actionString.IndexOf("(") - 1;
                var provinceName = actionString.Substring(0, endNameIndex).Trim();

                var startLandIndex = actionString.IndexOf("recaptured") + "recaptured".Length;
                var landTempString = actionString.Substring(startLandIndex);

                var splitted = landTempString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                int land = int.Parse(splitted[0]);

                var action = new AmbushMadeAction
                {
                    Date = utoDate,
                    ActionString = actionString,
                    Land = land
                };

                var province = Provinces.SingleOrDefault(prov => prov.ProvinceName == provinceName);
                if (province == null)
                {
                    province = new Province(provinceName);
                    Provinces.Add(province);
                }
                province.AddAmbushMade(action);
            }
        }

        private void AidAction(UtopiaDate utoDate, string actionString)
        {
            var actionIdentifier = " has sent an aid shipment to ";
            var sender = actionString.Substring(0, actionString.IndexOf(actionIdentifier));

            var receiver = actionString.Substring(actionString.IndexOf(actionIdentifier) + actionIdentifier.Length);
            if (receiver.EndsWith("."))
            {
                receiver = receiver.Substring(0, receiver.Length - 1);
            }

            var action = new AidAction
            {
                Date = utoDate,
                ActionString = actionString,
                Sender = sender,
                Receiver = receiver
            };

            var senderProvince = Provinces.SingleOrDefault(province => province.ProvinceName == sender);
            if (senderProvince == null)
            {
                senderProvince = new Province(sender);
                Provinces.Add(senderProvince);
            }
            senderProvince.AddAidSent(action);

            var receiverProvince = Provinces.SingleOrDefault(province => province.ProvinceName == receiver);
            if (receiverProvince == null)
            {
                receiverProvince = new Province(receiver);
                Provinces.Add(receiverProvince);
            }
            receiverProvince.AddAidReceived(action);
        }

        private void GenericAction(UtopiaDate utoDate, string actionString)
        {
            var action = new UnknownAction
            {
                Date = utoDate,
                ActionString = actionString
            };

            UnknownActions.Add(action);
        }

        private ActionType GetActionType(string actionString)
        {
            if (actionString.Contains("has sent an aid shipment to"))
            {
                return ActionType.Aid;
            }
            if (actionString.Contains(" recaptured "))
            {
                return ActionType.Ambush;
            }
            if (actionString.Contains(" ambushed armies from "))
            {
                return ActionType.Ambush;
            }
            if (actionString.Contains("(6:18), captured "))
            {
                return ActionType.Conquest;
            }
            if (actionString.Contains("(6:18) captured "))
            {
                return ActionType.TraditionalMarch;
            }
            if (actionString.Contains(") invaded ") && actionString.Contains("captured"))
            {
                return ActionType.TraditionalMarch;
            }
            if (actionString.Contains("razed"))
            {
                return ActionType.Raze;
            }
            if (actionString.Contains("killed"))
            {
                return ActionType.Massacre;
            }
            if (actionString.Contains(" has slain the dragon ravaging our lands!"))
            {
                return ActionType.DragonSlay;
            }
            if (actionString.Contains("attempted an invasion"))
            {
                return ActionType.Failed;
            }
            if (actionString.Contains("attempted to invade"))
            {
                return ActionType.Failed;
            }
            if (actionString.Contains("attacked and stole") || actionString.Contains("invaded and stole"))
            {
                return ActionType.Learn;
            }
            if (actionString.Contains("invaded and pillaged") || actionString.Contains("attacked and pillaged the lands"))
            {
                return ActionType.Plunder;
            }
            if (actionString.Contains("proposed a ceasefire offer") || actionString.Contains("has accepted our ceasefire proposal!") || actionString.Contains("has proposed a formal ceasefire with our kingdom") || actionString.Contains("We have entered into a formal ceasefire") || actionString.Contains("We have cancelled our ceasefire") || actionString.Contains("We have rejected a ceasefire offer") || actionString.Contains("has broken their ceasefire agreement with us!"))
            {
                return ActionType.CeaseFire;
            }
            if (actionString.Contains("has declared WAR with our kingdom") || actionString.Contains("We have declared WAR on "))
            {
                return ActionType.WarDeclare;
            }
            if (actionString.Contains("has withdrawn from war. Our people rejoice at our victory") || actionString.Contains("Unable to achieve victory, our Kingdom has withdrawn"))
            {
                return ActionType.WarEnd;
            }
            if (actionString.Contains(" Dragon project against us!") || actionString.Contains("Dragon project targetted at ") || actionString.Contains(" has begun ravaging our lands!") || actionString.Contains("Our dragon has set flight to ravage") || actionString.Contains("Our kingdom has cancelled the dragon project") || actionString.Contains("has cancelled their dragon project targetted at us."))
            {
                return ActionType.Dragon;
            }
            if (actionString.Contains("he lords of Utopia grant this kingdom a new opportunity") || actionString.Contains("Our monarch has forged an opportunity") || actionString.Contains("Our kingdom is strengthened by a new province") || actionString.Contains("Staying in the darkest shadows, ") || actionString.Contains("A new recruit has accepted our invitation") || actionString.Contains("Our monarch feels that this glorious kingdom is tarnished"))
            {
                return ActionType.KdManagement;
            }

            return ActionType.Unknown;
        }

        private UtopiaDate GetUtoDate(string action)
        {
            var date = new UtopiaDate();
            var separated = action.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            var month = separated[0];

            switch (month)
            {
                case "January":
                    date.Month = UtopiaMonthEnum.January;
                    break;
                case "February":
                    date.Month = UtopiaMonthEnum.February;
                    break;
                case "March":
                    date.Month = UtopiaMonthEnum.March;
                    break;
                case "April":
                    date.Month = UtopiaMonthEnum.April;
                    break;
                case "May":
                    date.Month = UtopiaMonthEnum.May;
                    break;
                case "June":
                    date.Month = UtopiaMonthEnum.June;
                    break;
                case "July":
                    date.Month = UtopiaMonthEnum.July;
                    break;
                default:
                    throw new Exception("Unknown month: " + month);
            }

            var dayString = separated[1];
            int day;
            if (int.TryParse(dayString, out day))
            {
                date.Day = day;
            }
            else
            {
                throw new Exception("Unknown day: " + dayString);
            }

            var yearString = separated[3].Replace("YR", "");
            int year;
            if (int.TryParse(yearString, out year))
            {
                date.Year = year;
            }
            else
            {
                throw new Exception("Unknown year: " + yearString);
            }

            return date;
        }

        public bool IsMartyr(string provinceName)
        {
            return Provinces.OrderBy(a => a.NetGain()).Take(3).Any(p => p.ProvinceName == provinceName);
        }

        public bool IsLuckyBastard(string provinceName)
        {
            return Provinces.OrderByDescending(a => a.NetGain()).Take(3).Any(p => p.ProvinceName == provinceName);
        }

        public bool IsHeavyWeightChampion(string provinceName)
        {
            return Provinces.OrderByDescending(a => int.Parse(a.AverageUniqueGain())).Take(1).Any(p => p.ProvinceName == provinceName);
        }

        public bool IsOctopusGeneral(string provinceName)
        {
            return Provinces.OrderByDescending(a => a.AttacksMade).Take(1).Any(p => p.ProvinceName == provinceName);
        }

        public bool IsPoorSoul(string provinceName)
        {
            return Provinces.OrderByDescending(a => a.AttacksReceived).Take(1).Any(p => p.ProvinceName == provinceName);
        }

        public bool IsNoLifer(string provinceName)
        {
            var max = Provinces.Max(p => p.NumberOfUniques());
            return Provinces.Where(p => p.NumberOfUniques() == max).Any(p => p.ProvinceName == provinceName);
        }

        public bool IsAlarm(string provinceName)
        {
            return Provinces.OrderBy(a => a.TimeBetweenUniques()).Take(1).Any(p => p.ProvinceName == provinceName);
        }

        public bool IsAssistant(string provinceName)
        {
            return Provinces.OrderByDescending(a => a.AidSent).Take(1).Any(p => p.ProvinceName == provinceName);
        }

        public bool IsLeecher(string provinceName)
        {
            return Provinces.OrderByDescending(a => a.AidReceived).Take(1).Any(p => p.ProvinceName == provinceName);
        }

        public bool IsOnlineAsOrdered(string provinceName)
        {
            var province = Provinces.FirstOrDefault(p => p.ProvinceName == provinceName);

            if (province != null)
            {
                var firstDate = WarStart.GetDayNumber() - 1;
                var lastDate = WarStart.GetDayNumber() + 1;

                return
                    province.AllActions.Where(
                        a => a is AttackMadeAction || (a is AidAction && ((AidAction) a).Receiver == provinceName))
                        .Any(a => a.Date.GetDayNumber() >= firstDate && a.Date.GetDayNumber() <= lastDate);
            }

            return false;
        }
    }
}