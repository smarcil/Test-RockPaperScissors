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
            //set the config
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

                //set round number
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

                // set player 1 type // Human by default
                ViewBag.Player1Type = Request.Form["Player1Type"];
                Config.Mouve Player1Choice = ChooseMovement(ViewBag.Player1Type);
                ViewBag.Player1Choice = Player1Choice.IDName;

                // set player 2 type // com player
                ViewBag.Player2Type = Request.Form["Player2Type"];
                Config.Mouve Player2Choice = ChooseMovement(ViewBag.Player2Type);
                ViewBag.Player2Choice = Player2Choice.IDName;

                checkWinner(Player1Choice, Player2Choice);
            }
            else
            {
                // if direct connection
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
            // get last choice
            string lastPlay = Request.Form["lastPlay2"];
            
            // create stronger move array if more than 3 choices
            List<Config.Mouve> mouves = new List<Config.Mouve>();
            foreach (Config.Mouve m in config.mouves)
            {
                foreach(string str in m.StrongerThan)
                {
                    if(lastPlay == str)
                    {
                        mouves.Add(m);
                        break;
                    }
                }
            }

            // if list is empty do RandomAI behaviour
            if (mouves.Count == 0) return RandomAIChoice();

            // choice inside the array
            Random r = new Random();
            int rInt = r.Next(0, mouves.Count);
            return mouves[rInt];
        }

        void checkWinner(Config.Mouve mouve1, Config.Mouve mouve2)
        {
            /*
             This behavior allows to have movement which is worth more powerful than other
             */

            int pointCount = 0;// < than 0 is player 1 if > than 0 is player 2
            if(ViewBag.winnerMatch == null)
            {
                ViewBag.winnerMatch = 0;
            }

            //check player 1 score
            foreach(string StrongerThan in mouve1.StrongerThan)
            {
                if (StrongerThan == mouve2.IDName)
                {
                    pointCount--;
                }
            }

            //check player 2 score
            foreach(string StrongerThan in mouve2.StrongerThan)
            {
                if (StrongerThan == mouve1.IDName)
                {
                    pointCount++;
                }
            }

            // set last winner and total match score
            if(pointCount != 0)
            {
                ViewBag.winner = pointCount < 0?1:2;
                ViewBag.winnerMatch += ViewBag.winner == 1 ? -1 : 1;
            }
        }
    }
}