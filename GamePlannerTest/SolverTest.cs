using Microsoft.VisualStudio.TestTools.UnitTesting;
using GamePlanner.Model;
using System.Collections.Generic;
using GamePlanner;
using System.Linq;

namespace GamePlannerTest
{
    [TestClass]
    public class SolverTest
    {
        private static List<Game> _gameLibrary = new List<Game>()
        {
            new Game() {Id = 1, MinPlayer = 2, MaxPlayer = 5, Name = "Carcassonne" },
            new Game() {Id = 2, MinPlayer = 2, MaxPlayer = 4, Name = "Splendor" },
            new Game() {Id = 3, MinPlayer = 2, MaxPlayer = 4, Name = "Through The Desert" },
            new Game() {Id = 4, MinPlayer = 3, MaxPlayer = 5, Name = "Settlers of Catan" },
            new Game() {Id = 5, MinPlayer = 2, MaxPlayer = 4, Name = "Race for the Galaxy" },
            new Game() {Id = 6, MinPlayer = 2, MaxPlayer = 4, Name = "Dominion" }
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


        private List<Player> CreateSixPlayerList()
        {
            int id = 1;
            var players = new List<Player>();
            players.Add(new Player(id++, "Alice", _prefersCarcassonne));
            players.Add(new Player(id++, "Bob", _prefersCarcassonne));
            players.Add(new Player(id++, "Eve", _prefersCarcassonne));
            players.Add(new Player(id++, "Dan", _prefersSplendor));
            players.Add(new Player(id++, "Erica", _prefersSplendor));
            players.Add(new Player(id++, "Nicholas", _prefersSplendor));

            return players;
        }

        private Solution FindSolutionForPlayers(List<Player> players, bool shuffle)
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
                    Assert.IsTrue(assignment.Players.Find(p => p.Name == "Dan") != null);
                    Assert.IsTrue(assignment.Players.Find(p => p.Name == "Erica") != null);
                    Assert.IsTrue(assignment.Players.Find(p => p.Name == "Nicholas") != null);
                }
                else if (assignment.Game.Name == "Carcassonne")
                {
                    Assert.IsTrue(assignment.Players.Find(p => p.Name == "Alice") != null);
                    Assert.IsTrue(assignment.Players.Find(p => p.Name == "Bob") != null);
                    Assert.IsTrue(assignment.Players.Find(p => p.Name == "Eve") != null);
                }
            }

        }

        [TestMethod]
        public void EightPlayersNeedSwaps()
        {
            int id = 1;
            var players = new List<Player>();
            players.Add(new Player(id++, "Alice", _prefersCarcassonne));
            players.Add(new Player(id++, "Dan", _prefersSplendor));
            players.Add(new Player(id++, "Bob", _prefersCarcassonne));
            players.Add(new Player(id++, "Erica", _prefersSplendor));
            players.Add(new Player(id++, "Eve", _prefersCarcassonne));
            players.Add(new Player(id++, "Nicholas", _prefersSplendor));
            players.Add(new Player(id++, "Frank", _prefersCarcassonne));
            players.Add(new Player(id++, "George", _prefersCarcassonne));

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
            int id = players[players.Count - 1].Id;
            players.Add(new Player(id, "Unhappy")
            {
                Preferences = new List<Preference>()
                {
                    new Preference(_gameLibrary[3], PrefOneWeight),
                    new Preference(_gameLibrary[4], PrefTwoWeight),
                    new Preference(_gameLibrary[5], PrefThreeWeight)
                }
            });

            var solution = FindSolutionForPlayers(players,false);
            Assert.IsTrue(solution.IsValid());

            Assert.AreEqual(2, solution.Assignments.Count);
            foreach (var assignment in solution.Assignments)
            {
                if (assignment.Game.Name == "Splendor")
                {
                    Assert.IsTrue(assignment.Players.Find(p => p.Name == "Dan") != null);
                    Assert.IsTrue(assignment.Players.Find(p => p.Name == "Erica") != null);
                    Assert.IsTrue(assignment.Players.Find(p => p.Name == "Nicholas") != null);
                }
                else if (assignment.Game.Name == "Carcassonne")
                {
                    Assert.IsTrue(assignment.Players.Find(p => p.Name == "Alice") != null);
                    Assert.IsTrue(assignment.Players.Find(p => p.Name == "Bob") != null);
                    Assert.IsTrue(assignment.Players.Find(p => p.Name == "Eve") != null);
                }
            }
        }
    }
}
