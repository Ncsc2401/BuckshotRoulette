using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BuckshotRoulette
{
    internal enum ItemType
    {
        DoubleDamage ,
        Block ,
        Heal ,
        Discart,
    }

    internal static class ItemRandomizer
    { 
        private static Random rand = new Random();

        private static int itemAmount = Enum.GetNames(typeof(ItemType)).Length;

        public static IItem GetRandomItem()
        {
            return GetItem(rand.Next(itemAmount));
        }

        private static IItem GetItem(int i)
        {
            ItemType item = (ItemType)i;
            switch (item)
            {
                case ItemType.DoubleDamage: return new DoubleDamage();
                case ItemType.Block: return new Block();
                case ItemType.Heal: return new Heal();
                case ItemType.Discart: return new Discart();
                default: throw new ArgumentException("Invalid item");
            }
        }
    }

    internal interface IItem
    {
        public string? Name { get; }

        public bool NeedsTarget { get; }

        public bool Effect(Player? source = null, Player? target = null); 
    }

    internal class DoubleDamage : IItem
    {
        public string? Name { get; } = "Double Damage";
        public bool NeedsTarget { get; } = false;

        public bool Effect(Player? source = null, Player? target = null)
        {
            Gun.Damage *= 2;
            return true;
        }
    }

    internal class Block : IItem
    {
        public string? Name { get; } = "Block";
        public bool NeedsTarget { get; } = true;

        public bool Effect(Player? source = null, Player ? target = null)
        {
            if (target is not null)
            {
                if (!target.IsBlocked)
                {
                    target.IsBlocked = true;
                    return true;
                }
            }
            return false;
        }
    }

    internal class Heal : IItem
    {
        public string? Name { get; } = "Heal";
        public bool NeedsTarget { get; } = false;

        public bool Effect(Player? source = null, Player? target = null)
        {
            if (source is not null)
            {
                source.Lifes++;
                return true;
            }
            return false;
        }
    }

    internal class Discart : IItem
    {
        public string? Name { get; } = "Discart";
        public bool NeedsTarget { get; } = false;

        public bool Effect(Player? source = null, Player? target = null)
        {
            if (!Gun.IsEmpty())
            {
                Gun.Shoot();
                return true;
            }
            return false;
        }
    }
}
