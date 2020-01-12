using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Collections.Specialized;

using Moq;

using Rock_Paper_Scissors.Controllers;
using Rock_Paper_Scissors.Utilities;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_ChooseMovement_TacticalAI()
        {
            NewGameController game = new NewGameController();
            game.config = new Config()
            {
                   NumberOfMatch=3,
                   mouves= new Config.Mouve[]
                   {
                       new Config.Mouve(){IDName="Rock", StrongerThan=new string[]{ "Scissors" } },
                       new Config.Mouve(){IDName="Scissors", StrongerThan=new string[]{ "Paper" } },
                       new Config.Mouve(){IDName="Paper", StrongerThan=new string[]{ "Rock" } }
                   }
            };

            NameValueCollection form = new NameValueCollection();
            game.lastPlay2 = "Rock";

            Config.Mouve mouve = game.ChooseMovement("TacticalAI");
            Assert.AreEqual(mouve.IDName, "Paper");

            game.lastPlay2 = "Scissors";

            mouve = game.ChooseMovement("TacticalAI");
            Assert.AreEqual(mouve.IDName, "Rock");

            game.lastPlay2 = "Paper";

            mouve = game.ChooseMovement("TacticalAI");
            Assert.AreEqual(mouve.IDName, "Scissors");
        }

        [TestMethod]
        public void Test_checkWinner()
        {
            NewGameController game = new NewGameController();
            game.config = new Config()
            {
                NumberOfMatch = 3,
                mouves = new Config.Mouve[]
                   {
                       new Config.Mouve(){IDName="Rock", StrongerThan=new string[]{ "Scissors" } },
                       new Config.Mouve(){IDName="Scissors", StrongerThan=new string[]{ "Paper" } },
                       new Config.Mouve(){IDName="Paper", StrongerThan=new string[]{ "Rock" } }
                   }
            };

            game.ViewBag.winner = 0;
            game.ViewBag.winnerMatch = 0;

            game.checkWinner(game.config.mouves[0], game.config.mouves[0]);
            Assert.AreEqual(game.ViewBag.winner, 0);
            Assert.AreEqual(game.ViewBag.winnerMatch, 0);
            game.checkWinner(game.config.mouves[1], game.config.mouves[1]);
            Assert.AreEqual(game.ViewBag.winner, 0);
            Assert.AreEqual(game.ViewBag.winnerMatch, 0);
            game.checkWinner(game.config.mouves[2], game.config.mouves[2]);
            Assert.AreEqual(game.ViewBag.winner, 0);
            Assert.AreEqual(game.ViewBag.winnerMatch, 0);

            game.checkWinner(game.config.mouves[0], game.config.mouves[1]);
            Assert.AreEqual(game.ViewBag.winner, 1);
            Assert.AreEqual(game.ViewBag.winnerMatch, -1);

            game.checkWinner(game.config.mouves[0], game.config.mouves[2]);
            Assert.AreEqual(game.ViewBag.winner, 2);
            Assert.AreEqual(game.ViewBag.winnerMatch, 0);

            game.checkWinner(game.config.mouves[1], game.config.mouves[2]);
            Assert.AreEqual(game.ViewBag.winner, 1);
            Assert.AreEqual(game.ViewBag.winnerMatch, -1);
        }
    }
}
