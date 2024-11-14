using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BuckshotRoulette
{
    internal static class GameHandler
    {
        private static List<Player> players = new();

        private static void PlayerOptions(Player player)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Choose an option");
            sb.AppendLine("1 - Shoot yourself");
            sb.AppendLine("2 - Shoot someone");
            sb.AppendLine("3 - Use item");
            Console.Write(sb.ToString());

            int option = Convert.ToInt32(Console.ReadLine());
            switch (option)
            {
                //Shoot yourself
                case 1:
                    if (Gun.Shoot())
                    {
                        Console.Clear();
                        Console.WriteLine("Bang!");
                        Console.WriteLine("Pass to the next player");
                        Console.ReadKey();
                        Console.Clear();
                        player.Lifes -= Gun.Damage;
                        Gun.Damage = Gun.DefaultDamage;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Pshiu...");
                        Console.WriteLine("You have another chance");
                        Console.ReadKey();
                        Console.Clear();
                        Gun.Damage = Gun.DefaultDamage;
                        PlayerTurn(player);
                    }
                    break;

                //Shot someone
                case 2:
                    Console.Clear();

                    PrintPlayers(player);
                    int j = Convert.ToInt32(Console.ReadLine());

                    if (Gun.Shoot())
                    {
                        Console.Clear();
                        Console.WriteLine("Bang!");
                        Console.WriteLine("Pass to the next player");
                        Console.ReadKey();
                        Console.Clear();
                        players[j].Lifes -= Gun.Damage;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Pshiu!");
                        Console.WriteLine("Pass to the next player");
                        Console.ReadKey();
                        Console.Clear();
                    }

                    Gun.Damage = Gun.DefaultDamage;

                    break;

                //Show ItemType
                case 3:
                    Console.Clear();

                    Console.WriteLine(player.InventoryString());
                    int i = Convert.ToInt32(Console.ReadLine());

                    Player? target = null;

                    if (player.inventory[i].NeedsTarget)
                    {
                        Console.Clear();
                        PrintPlayers(player);
                        int targetId = Convert.ToInt32(Console.ReadLine());
                        target = players[targetId];
                    }

                    player.UseItem(i, target);

                    PlayerTurn(player);

                    break;
                default:
                    throw new ArgumentException("Invalid option");
            }
        }

        private static void Game()
        {
            foreach (var player in players)
            {
                if (player.Lifes > 0 && !player.IsBlocked)
                {
                    player.GetItem();
                    PlayerTurn(player);
                }
                if (player.IsBlocked)
                {
                    player.IsBlocked = false;
                }
                if (CheckWinner() is not null)
                {
                    break;
                }
            }
            if (CheckWinner() is not null)
            {
                Console.Clear();
                Console.WriteLine(CheckWinner()?.Name + " won!");
            }
            else
            {
                Game();
            }
        }

        private static Player? CheckWinner()
        {
            int alive = 0;
            foreach (var player in players)
            {
                if (player.Lifes > 0)
                {
                    alive++;
                }
            }
 
            if (alive == 1)
            {
                foreach (var player in players)
                {
                    if (player.Lifes > 0)
                    {
                        return player;
                    }
                }
            }

            return null;
        }

        private static void PlayerTurn(Player player)
        {
            try
            {
                Console.Clear();
                Console.WriteLine(player);
                Console.WriteLine(Gun.ToString());
                PlayerOptions(player);
            }

            catch (Exception e)
            {
                if (e is ArgumentException || e is FormatException)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid option");
                    PlayerTurn(player);
                }
                else
                {
                    throw;
                }
            }
        }

        public static void Start()
        {
            Console.Clear();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("1 - Start");
            sb.AppendLine("2 - Exit");
            Console.WriteLine(sb.ToString());
            try
            {
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        GetPlayers();
                        break;
                    case 2:
                        break;
                    default:
                        throw new ArgumentException("Invalid option");
                }
            }
            catch (Exception e)
            {
                if (e is ArgumentException || e is FormatException)
                {
                    Console.WriteLine("Invalid option");
                    Start();
                }
                else
                {
                    throw;
                }
            }
        }

        private static void PrintPlayers()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Lifes > 0)
                {
                    sb.Append(i);
                    sb.Append(" : ");
                    sb.AppendLine(players[i].ToString());
                }
            }
            Console.Write(sb.ToString());
        }

        private static void PrintPlayers(Player player)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] != player && players[i].Lifes > 0){
                    sb.Append(i);
                    sb.Append(" : ");
                    sb.AppendLine(players[i].ToString());
                }
            }
            Console.Write(sb.ToString());
        }

        private static void PrintPlayer(int i)
        {
            Console.WriteLine(players[i]);
        }

        private static void GetPlayers()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("How many players?");
                int n = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                if (n > 1)
                {
                    for (int i = 0; i < n; i++)
                    {
                        Console.WriteLine("Name: ");
                        String? name = Console.ReadLine();
                        players.Add(new Player(!(name == "" || name is null) ? name : $"Player {i}"));
                        Console.Clear();
                    }
                    Game();
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception e)
            {
                if (e is FormatException || e is ArgumentException)
                {
                    Console.WriteLine("Invalid number");
                    Console.ReadKey();
                    GetPlayers();
                }
                else
                {
                    throw;
                }
            }
        }

    }
}
