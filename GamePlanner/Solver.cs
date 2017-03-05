using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;
using GamePlannerModel;

namespace GamePlanner
{
    public class Solver
    {
        //https://en.m.wikipedia.org/wiki/Stable_roommates_problem

        public List<Solution> FindSolutions( Event gameNight, bool shuffleLists )
        {
            var solutions = new List<Solution>();

            var potentialGames = CreateListOfPotentialGames(gameNight);

            int solutionCount = 1;
            if (shuffleLists)
                solutionCount = gameNight.Players.Count;

            for (int i = 0; i < solutionCount; i++)
            {
                Debug.WriteLine("Creating Solution for Event. Iteration:"+i);
                var sln = CreateSolution(gameNight, potentialGames, shuffleLists);
                solutions.Add(sln);
                Debug.WriteLine("Solution TotalSatisfaction:{0}; Avg:{1}; Mean:{2}", sln.TotalSatisfaction, sln.AverageSatisfaction, sln.MeanSatisfaction);
                Debug.WriteLine("-------------------------------------------------");
            }

            if (solutions.Count > 1)
            {
                solutions.Sort((x, y) => x.MeanSatisfaction.CompareTo(y.MeanSatisfaction));
                solutions.Reverse();
            }

            return solutions;
        }

        private static Solution CreateSolution(Event gameNight, List<Game> potentialGames, bool shuffleLists)
        {
            var solution = new Solution();
            var players = new List<EventRegistration>(gameNight.Players);

            if (shuffleLists)
            {
                Shuffle(potentialGames);
                Shuffle(players);
            }
            var playerList = new List<EventRegistration>(players);

            // phase 1 - create initial assignments
            var assignments = CreateInitialAssignments(gameNight, potentialGames, players);
            foreach ( var a in assignments )
            {
                Debug.WriteLine("Initial Assignment: " + a.ToString());
            }

            // phase 2 - foreach player, does a better match exist, if so switch to that game
            for (int i = 0; i < playerList.Count; i++)
            {
                if (!PerformRotations(assignments, playerList))
                    break;
            }


            // phase 3 - ensure each assignment is valid
            FixInvalidGames(assignments);

            foreach(var a in assignments)
            {
                Debug.WriteLine("Final Assignment: " + a);
            }

            solution.Assignments = assignments;

            // phase 4 - calculate satisfaction
            CalculateSatisfaction(solution);

            return solution;
        }


        private static List<GameAssignment> CreateInitialAssignments(Event gameNight, List<Game> potentialGames, List<EventRegistration> players)
        {
            var assignments = new Dictionary<int, GameAssignment>();
            foreach (var game in potentialGames)
            {
                assignments[game.ID] = new GameAssignment() { Event = gameNight, Game = game, Players = new List<EventRegistration>() };
            }
                            
            foreach (var game in potentialGames)
            {
                for (int p = 0; p < players.Count; p++)
                {
                    EventRegistration player = players[p];
                    if (player.HasPreferenceForGame(game))
                    {
                        var a = assignments[game.ID];
                        if( a.CanAddPlayer())
                        {
                            AssignPlayer(players, player, a);
                            p--;
                        }
                    }
                }
            }

            if (players.Count > 0)
            {
                throw new ArgumentException("Failed to assign all players to games in phase one. Remaining player count:" + players.Count);
            }

            return new List<GameAssignment>(assignments.Values);
        }


        private static bool PerformRotations(List<GameAssignment> assignments, List<EventRegistration> players)
        {
            bool result = false;

            foreach( var player in players)
            {
                var higherPrefs = new List<Preference>();                
                foreach( var pref in player.Preferences )
                {
                    if (pref.Weight > player.Satisfaction)
                        higherPrefs.Add(pref);
                }

                if (higherPrefs.Count > 0)
                {
                    higherPrefs.Sort((x, y) => x.Weight.CompareTo(y.Weight));

                    foreach (var p in higherPrefs)
                    {
                        bool swapped = false;
                        var potentialSwaps = assignments.Where(a => a.Game.ID == p.Game.ID);
                        foreach( var swap in potentialSwaps )
                        {
                            Rotation rotation = IdentifyRotation(player, swap);

                            result = rotation.RotationType != RotationType.NoRotationPossible;

                            if ( rotation.RotationType == RotationType.MovePlayer )
                            {
                                player.Assignment.Players.Remove(player);
                                swap.Players.Add(player);
                                player.Assignment = swap;

                                Debug.WriteLine("Rotation - Moved {0} to Game:{1}", player.User.Name, swap.Game.Name);                                
                            }
                            else if( rotation.RotationType == RotationType.TradePlayers )
                            {
                                GameAssignment originalAssignment = player.Assignment;

                                player.Assignment.Players.Remove(player);
                                swap.Players.Add(player);
                                player.Assignment = swap;

                                swap.Players.Remove(rotation.PlayerTwo);
                                originalAssignment.Players.Add(rotation.PlayerTwo);
                                rotation.PlayerTwo.Assignment = originalAssignment;

                                Debug.WriteLine("Rotation - Traded {0} to Game:{1} and {2} to Game:{3}", player.User.Name, swap.Game.Name, rotation.PlayerTwo.User.Name, originalAssignment.Game.Name);
                            }
                        }

                        if (swapped)
                            break;
                    }
                }
            }

            return result;
        }


        private static Rotation IdentifyRotation(EventRegistration player, GameAssignment swap)
        {
            if (swap.CanAddPlayer())
            {
                return new Rotation(RotationType.MovePlayer);
            }
            else
            {
                foreach (var p in swap.Players)
                {
                    if (p.PrefersGameOverCurrentAssignment(player.Assignment.Game))
                    {
                        return new Rotation(RotationType.TradePlayers)
                        {
                            PlayerTwo = p
                        };
                    }
                }
            }

            return Rotation.NoRotation;
        }
        

        private static void FixInvalidGames(List<GameAssignment> assignments)
        {
            for(int i = 0; i < assignments.Count; i++)
            {
                GameAssignment a = assignments[i];
                if( a.Players.Count == 0 )
                {
                    assignments.Remove(a);
                    i--;
                }
            }

            assignments.Sort((x, y) => x.GetTotalSatisfaction().CompareTo(y.GetTotalSatisfaction()));
            assignments.Reverse();

            for(int i = 0; i < assignments.Count; i++)
            {                
                GameAssignment a = assignments[i];
                if (!a.IsValid())
                {
                    var unassignedPlayers = new List<EventRegistration>();

                    Debug.WriteLine("Removing Players from Invalid Assignment: " + a.ToString());
                    foreach (var p in a.Players)
                    {
                        p.Assignment = null;
                        unassignedPlayers.Add(p);
                    }

                    assignments.Remove(a);
                    i--;

                    for (int j = i; j < assignments.Count; j++ )
                    {
                        GameAssignment potentialNewAssignment = assignments[j];
                        for (int p = 0; p < unassignedPlayers.Count; p++)
                        {
                            EventRegistration unassigned = unassignedPlayers[p];
                            if (potentialNewAssignment.CanAddPlayer() 
                                && unassigned.HasPreferenceForGame(potentialNewAssignment.Game))
                            {
                                Debug.WriteLine("Moving unassigned player '{0}' to preferred game:{1}", unassigned.User.Name, potentialNewAssignment.Game );

                                potentialNewAssignment.Players.Add(unassigned);
                                unassigned.Assignment = potentialNewAssignment;
                                unassignedPlayers.Remove(unassigned);
                                p--;
                            }
                        }
                    }
                    
                    foreach(EventRegistration p in unassignedPlayers )
                    {
                        foreach( var assignment in assignments )
                        {
                            if( assignment.CanAddPlayer())
                            {
                                Debug.WriteLine("Moving unsatisfied player '{0}' to game:{1} (sorry!)", p.User.Name, assignment.Game);
                                assignment.Players.Add(p);
                                p.Assignment = assignment;
                                break;
                            }
                        }
                    }
                }
            }
        }


        private static void CalculateSatisfaction(Solution solution)
        {
            var satisfactions = new List<double>();
            foreach (var assignment in solution.Assignments)
            {
                foreach (var player in assignment.Players)
                {
                    satisfactions.Add(player.Satisfaction);
                }
            }
            solution.TotalSatisfaction = satisfactions.Sum();
            solution.AverageSatisfaction = satisfactions.Average();
            solution.MeanSatisfaction = Mean(satisfactions);
        }

        private static void AssignPlayer(List<EventRegistration> players, EventRegistration player, GameAssignment assignment)
        {
            assignment.Players.Add(player);
            player.Assignment = assignment;
            players.Remove(player);
        }

        private List<Game> CreateListOfPotentialGames(Event gameNight)
        {
            var games = new List<Game>();
            var gameSet = new Dictionary<int,Game>();
            
            foreach(var player in gameNight.Players )
            {
                foreach( var pref in player.Preferences )
                {
                    gameSet[pref.Game.ID] = pref.Game;
                }
            }

            foreach( var game in gameSet )
            {
                games.Add(game.Value);
            }

            return games;
        }

        private static Random _rng = new Random();
        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static double Mean(List<double> values)
        {
            int start = 0;
            int end = values.Count;
            double s = 0;

            for (int i = start; i < end; i++)
            {
                s += values[i];
            }

            return s / (end - start);
        }
    }
}
