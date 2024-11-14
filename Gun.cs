using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuckshotRoulette
{
    internal static class Gun
    {
        private static Random rand = new Random();

        public static int DefaultDamage { get; } = 1;
        private static int maxBullets = 9;
        private static float realChance = 0.35f;

        public static int Damage { get; set; } = 1;

        private static List<bool> bullets = Gun.LoadGun();

        public static List<bool> LoadGun()
        {
            List<bool> result = new();
            for (int i = 0; i < rand.Next(3, maxBullets); i++)
            {
                result.Add(rand.Next(0, 100) < realChance * 100);
            }
            if (!result.Contains(true))
            {
                result[0] = true;
            }
            return result;
        }

        public static bool Shoot()
        {
            bool bullet = bullets.First();
            bullets.RemoveAt(0);
            if (IsEmpty())
            {
                bullets = LoadGun();
            }
            return bullet;
        }

        public static bool IsEmpty()
        {
            return bullets.Count == 0;
        }

        public new static string ToString()
        {
            int real = 0;
            int fake = 0;
            foreach (var bullet in bullets)
            {
                if (bullet)
                {
                    real++;
                }
                else
                {
                    fake++;
                }
            }
            return $"{real + fake} bullets\n{real} real\n{fake} fake\n";
        }
    }
}
