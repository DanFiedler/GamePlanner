using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GamePlanner;
using System.Linq;
using GamePlannerModel;
using System;

namespace GamePlannerTest
{
    [TestClass]
    public class SolverTest
    {
        private static List<Game> _gameLibrary = new List<Game>()
        {
            new Game() {ID = 1, MinPlayer = 2, MaxPlayer = 5, Name = "Carcassonne" },
            new Game() {ID = 2, MinPlayer = 2, MaxPlayer = 4, Name = "Splendor" },
            new Game() {ID = 3, MinPlayer = 2, MaxPlayer = 4, Name = "Through The Desert" },
            new Game() {ID = 4, MinPlayer = 3, MaxPlayer = 5, Name = "Settlers of Catan" },
            new Game() {ID = 5, MinPlayer = 2, MaxPlayer = 4, Name = "Race for the Galaxy" },
            new Game() {ID = 6, MinPlayer = 2, MaxPlayer = 4, Name = "Dominion" }
        };

        private const double PrefOneWeight = 0.53;
        private const double PrefTwoWeight = 0.33;
        private const double PrefThreeWeight = 0.13;

        private static List<Preference> _prefersCarcassonne = new List<Preference>()
            {   new Preference( _gameLibrary[0], PrefOneWeight),
                new Preference( _gameLibrary[1], PrefTwoWeight),
                new Preference( _gameLibrary[2], PrefThreeWeight) };

        private static List<Preference> _prefersSplendor = new List<Preference>()
            {   new Preference( _gameLibrary[1], PrefOneWeight),
                new Preference( _gameLibrary[2], PrefTwoWeight),
                new Preference( _gameLibrary[0], PrefThreeWeight) };


        private List<EventRegistration> CreateSixPlayerList()
        {
            var players = new List<EventRegistration>();
            players.Add(new EventRegistration("Alice", _prefersCarcassonne));
            players.Add(new EventRegistration("Bob", _prefersCarcassonne));
            players.Add(new EventRegistration("Eve", _prefersCarcassonne));
            players.Add(new EventRegistration("Dan", _prefersSplendor));
            players.Add(new EventRegistration("Erica", _prefersSplendor));
            players.Add(new EventRegistration("Nicholas", _prefersSplendor));

            return players;
        }

        private Solution FindSolutionForPlayers(List<EventRegistration> players, bool shuffle)
        {
            var gameNight = new Event();
            gameNight.GameOptions = _gameLibrary;
            gameNight.Players = players;

            var solver = new Solver();
            var solutions = solver.FindSolutions(gameNight, shuffle);

            var solution = solutions[0];
            return solution;
        }

        //[TestMethod]
        //public void Shuffle()
        //{
        //    int id = 1;
        //    var players = new List<Player>()
        //    {
        //        new Player(id++,"Dan"), new Player(id++,"Erica"), new Player(id++,"Nicholas"), new Player(id++,"Anne"),
        //        new Player(id++,"Stephen"), new Player(id++,"Heidi"), new Player(id++,"Abe"), new Player(id++,"Kathleen")
        //    };
        //    var shuffledPlayers = new List<Player>(players);
        //    Solver.Shuffle(players);
        //    Assert.AreNotEqual(shuffledPlayers[0].Name, players[0].Name);
        //}


        [TestMethod]
        public void SixPlayersTwoGamesEasy()
        {
            SixPlayerTest(false);
        }

        [TestMethod]
        public void SixPlayersTwoGamesEasyWithShuffle()
        {
            SixPlayerTest(true);
        }

        private void SixPlayerTest( bool shuffle )
        {
            var players = CreateSixPlayerList();
            var solution = FindSolutionForPlayers(players, shuffle);
            Assert.IsTrue(solution.IsValid());

            var weightList = new List<double> { PrefOneWeight, PrefOneWeight, PrefOneWeight, PrefOneWeight, PrefOneWeight, PrefOneWeight };
            double expectedTotal = weightList.Sum();
            double expectedAvg = weightList.Average();
            Assert.AreEqual(expectedTotal, solution.TotalSatisfaction);
            Assert.AreEqual(expectedAvg, solution.AverageSatisfaction);

            Assert.AreEqual(2, solution.Assignments.Count);
            foreach (var assignment in solution.Assignments)
            {
                Assert.AreEqual(3, assignment.Players.Count);
                if (assignment.Game.Name == "Splendor")
                {
                    Assert.IsTrue(assignment.Players.Where(p => p.User.Name == "Dan") != null);
                    Assert.IsTrue(assignment.Players.Where(p => p.User.Name == "Erica") != null);
                    Assert.IsTrue(assignment.Players.Where(p => p.User.Name == "Nicholas") != null);
                }
                else if (assignment.Game.Name == "Carcassonne")
                {
                    Assert.IsTrue(assignment.Players.Where(p => p.User.Name == "Alice") != null);
                    Assert.IsTrue(assignment.Players.Where(p => p.User.Name == "Bob") != null);
                    Assert.IsTrue(assignment.Players.Where(p => p.User.Name == "Eve") != null);
                }
            }

        }

        [TestMethod]
        public void EightPlayersNeedSwaps()
        {
            var players = new List<EventRegistration>();
            players.Add(new EventRegistration("Alice", _prefersCarcassonne));
            players.Add(new EventRegistration("Dan", _prefersSplendor));
            players.Add(new EventRegistration("Bob", _prefersCarcassonne));
            players.Add(new EventRegistration("Erica", _prefersSplendor));
            players.Add(new EventRegistration("Eve", _prefersCarcassonne));
            players.Add(new EventRegistration("Nicholas", _prefersSplendor));
            players.Add(new EventRegistration("Frank", _prefersCarcassonne));
            players.Add(new EventRegistration("George", _prefersCarcassonne));

            var solution = FindSolutionForPlayers(players, false);
            Assert.IsTrue(solution.IsValid());

            var weightList = new List<double> { PrefOneWeight, PrefOneWeight, PrefOneWeight, PrefOneWeight, PrefOneWeight, PrefOneWeight, PrefOneWeight, PrefOneWeight };
            double expectedTotal = weightList.Sum();
            double expectedAvg = weightList.Average();
            Assert.AreEqual(expectedTotal, solution.TotalSatisfaction);
            Assert.AreEqual(expectedAvg, solution.AverageSatisfaction);
        }

        [TestMethod]
        public void SevenPlayersTwoGamesEnsureValidity()
        {
            var players = CreateSixPlayerList();
            players.Add(new EventRegistration("Unhappy", 
                new List<Preference>()
                {
                    new Preference(_gameLibrary[3], PrefOneWeight),
                    new Preference(_gameLibrary[4], PrefTwoWeight),
                    new Preference(_gameLibrary[5], PrefThreeWeight)
                }));

            var solution = FindSolutionForPlayers(players,false);
            Assert.IsTrue(solution.IsValid());

            Assert.AreEqual(2, solution.Assignments.Count);
            foreach (var assignment in solution.Assignments)
            {
                if (assignment.Game.Name == "Splendor")
                {
                    Assert.IsTrue(assignment.Players.Where(p => p.User.Name == "Dan") != null);
                    Assert.IsTrue(assignment.Players.Where(p => p.User.Name == "Erica") != null);
                    Assert.IsTrue(assignment.Players.Where(p => p.User.Name == "Nicholas") != null);
                }
                else if (assignment.Game.Name == "Carcassonne")
                {
                    Assert.IsTrue(assignment.Players.Where(p => p.User.Name == "Alice") != null);
                    Assert.IsTrue(assignment.Players.Where(p => p.User.Name == "Bob") != null);
                    Assert.IsTrue(assignment.Players.Where(p => p.User.Name == "Eve") != null);
                }
            }
        }

        public Event SetupFamilyGameGroupNumberOne()
        {
            int id = 1;
            var loveLetter = new Game(id++, "Love Letter", 2, 4);
            var applesToApples = new Game(id++, "Apples to Apples", 4, 10);
            var puertoRico = new Game(id++, "Puerto Rico", 2, 5);
            var dominion = new Game(id++, "Dominion", 2, 4);
            var splendor = new Game(id++, "Splendor", 2, 4);
            var citadels = new Game(id++, "Citadels", 2, 8);
            var sanJuan = new Game(id++, "San Juan", 2, 4);
            var tahiti = new Game(id++, "Tahiti", 2, 4);
            var catan = new Game(id++, "Settlers of Catan", 3, 4);
            var pandemic = new Game(id++, "Pandemic", 2, 4);
            var blokus = new Game(id++, "Blokus", 4, 4);

            var gameOptions = new List<Game>()
            {
                loveLetter, applesToApples, puertoRico, dominion, splendor,
                citadels, sanJuan, tahiti, catan, pandemic, blokus
            };

            var players = new List<EventRegistration>();
            players.Add(new EventRegistration("Laurel", new List<Preference>()
            {
                new Preference(loveLetter, PrefOneWeight), new Preference(applesToApples, PrefTwoWeight), new Preference(puertoRico, PrefThreeWeight)
            }));

            players.Add(new EventRegistration("Dan", new List<Preference>()
            {
                new Preference(dominion, PrefOneWeight), new Preference(loveLetter, PrefTwoWeight), new Preference(splendor, PrefThreeWeight)
            }));

            players.Add(new EventRegistration("Kathleen", new List<Preference>()
            {
                new Preference(citadels, PrefOneWeight), new Preference(sanJuan, PrefTwoWeight), new Preference(tahiti, PrefThreeWeight)
            }));

            players.Add(new EventRegistration("Abe", new List<Preference>()
            {
                new Preference(puertoRico, PrefOneWeight), new Preference(catan, PrefTwoWeight), new Preference(pandemic, PrefThreeWeight)
            }));

            players.Add(new EventRegistration("Anne", new List<Preference>()
            {
                new Preference(loveLetter, PrefOneWeight), new Preference(dominion, PrefTwoWeight), new Preference(blokus, PrefThreeWeight)
            }));

            players.Add(new EventRegistration("Nicholas", new List<Preference>()
            {
                new Preference(splendor, PrefOneWeight), new Preference(dominion, PrefTwoWeight), new Preference(puertoRico, PrefThreeWeight)
            }));

            players.Add(new EventRegistration("Erica", new List<Preference>()
            {
                new Preference(dominion, PrefOneWeight), new Preference(pandemic, PrefTwoWeight), new Preference(splendor, PrefThreeWeight)
            }));

            var gameNight = new Event();
            gameNight.GameOptions = gameOptions;
            gameNight.Players = players;

            return gameNight;
        }

        [TestMethod]
        public void FamilyGameGroupNumberOne()
        {
            var gameNight = SetupFamilyGameGroupNumberOne();   

            var solver = new Solver();
            var solutions = solver.FindSolutions(gameNight, false);

            var solution = solutions[0];
            Assert.IsTrue(solution.IsValid());
        }

        [TestMethod]
        public void FamilyGameGroupNumberOneWithRandomizedIterations()
        {
            var gameNight = SetupFamilyGameGroupNumberOne();

            var solver = new Solver();
            var solutions = solver.FindSolutions(gameNight, true);           

            foreach (var solution in solutions)
            {
                Assert.IsTrue(solution.IsValid());

                System.Diagnostics.Debug.Write(solution.ToString());
            }
        }
    }
}
