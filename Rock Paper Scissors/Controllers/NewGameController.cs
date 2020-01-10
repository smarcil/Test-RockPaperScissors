using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Diagnostics;

using Newtonsoft.Json;

using Rock_Paper_Scissors.Utilities;

namespace Rock_Paper_Scissors.Controllers
{
    public class NewGameController : Controller
    {
        Config config;

        // GET: NewGame
        public ActionResult Index()
        {
            using (StreamReader sr = new StreamReader(Server.MapPath("~/Assets/Configs/config.json")))
            {
                config = JsonConvert.DeserializeObject<Config>(sr.ReadToEnd());
            }

            if (Request.HttpMethod == "POST")
            {
                if (Request.Form["FormName"] == "HomeForm")
                {
                    ViewBag.winnerMatch = 0;
                }
                else
                {
                    ViewBag.winnerMatch = int.Parse( Request.Form["winnerMatch"].ToString() );
                }

                int roundNumber;
                if(int.TryParse(Request.Form["roundNumber"], out roundNumber))
                {
                    roundNumber++;
                    ViewBag.roundNumber = roundNumber;
                }
                else
                {
                    ViewBag.roundNumber = 1;
                }

                ViewBag.Player1Type = Request.Form["Player1Type"];
                Config.Mouve Player1Choice = ChooseMovement(ViewBag.Player1Type);
                ViewBag.Player1Choice = Player1Choice.IDName;

                ViewBag.Player2Type = Request.Form["Player2Type"];
                Config.Mouve Player2Choice = ChooseMovement(ViewBag.Player2Type);
                ViewBag.Player2Choice = Player2Choice.IDName;

                checkWinner(Player1Choice, Player2Choice);
            }
            else
            {
                ViewBag.roundNumber = 1;
            }

            return View(config);
        }

        Config.Mouve ChooseMovement(string PlayerType)
        {
            switch (PlayerType)
            {
                case "Human": return config.mouves[int.Parse(Request.Form["mouve"])];
                case "RandomAI": return RandomAIChoice();
                case "TacticalAI": return TacticalAIChoice();
            }
            return RandomAIChoice();
        }

        Config.Mouve RandomAIChoice()
        {
            Random r = new Random();
            int rInt = r.Next(0, config.mouves.Length);
            return config.mouves[rInt];
        }

        Config.Mouve TacticalAIChoice()
        {
            Trace.WriteLine("asdsafasdfsadffsdas");
            return RandomAIChoice();
        }

        void checkWinner(Config.Mouve mouve1, Config.Mouve mouve2)
        {
            int pointCount = 0;
            if(ViewBag.winnerMatch == null)
            {
                ViewBag.winnerMatch = 0;
            }
            foreach(string StrongerThan in mouve1.StrongerThan)
            {
                if (StrongerThan == mouve2.IDName)
                {
                    pointCount--;
                }
            }
            foreach(string StrongerThan in mouve2.StrongerThan)
            {
                if (StrongerThan == mouve1.IDName)
                {
                    pointCount++;
                }
            }

            if(pointCount != 0)
            {
                ViewBag.winner = pointCount < 0?1:2;
                ViewBag.winnerMatch += ViewBag.winner == 1 ? -1 : 1;
            }
        }
    }
}