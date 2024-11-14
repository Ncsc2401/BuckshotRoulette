using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BuckshotRoulette
{
    internal class Player
    {
        public string? Name { get; set; }
        public int Lifes { get; set; } = 3;

        public bool IsBlocked { get; set; } = false;

        public List<IItem> inventory { get; } = new();

        private static Random rand = new Random();

        public Player(string name)
        {
            Name = name;
        }

        public IItem GetItem()
        {
            IItem item = ItemRandomizer.GetRandomItem();
            inventory.Add(item);
            return item;
        }
        public IItem GetItem(IItem item)
        {
            inventory.Add(item);
            return item;
        }

        public bool UseItem(int id, Player? target = null)
        {
            IItem item = inventory[id];
            if (item.Effect(this, target))
            {
                inventory.RemoveAt(id);
                return true;
            }
            return false;
        }

        public string InventoryString()
        {
            StringBuilder sb = new StringBuilder();
            if (inventory.Count > 0)
            {
                for (int i = 0; i < inventory.Count; i++)
                {
                    sb.Append(i);
                    sb.Append(" - ");
                    sb.AppendLine(inventory[i].Name);
                }
                return sb.ToString();
            }
            return "No Items";
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name);
            sb.Append(" - ");
            sb.Append(Lifes);
            sb.Append(" Lifes");
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
