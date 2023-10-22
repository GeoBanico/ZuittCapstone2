using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capstone.Users
{
    internal class Item
    {
        private int id;
        private string name;
        private string branch;
        private int beginningInventory;
        private int stockIn;
        private int stockOut;
        private int totalBalance;

        public Item(int id, string name, string branch, int beginningInventory, int stockIn, int stockOut)
        {
            this.id = id;
            this.name = name;
            this.branch = branch;
            this.beginningInventory = beginningInventory;
            this.stockIn = stockIn;
            this.stockOut = stockOut;
            this.totalBalance = beginningInventory + stockIn - stockOut;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Branch { get => branch; set => branch = value; }
        public int BeginningInventory { get => beginningInventory; set => beginningInventory = value; }
        public int StockIn { get => stockIn; set => stockIn = value; }
        public int StockOut { get => stockOut; set => stockOut = value; }
        public int TotalBalance { get => totalBalance; set => totalBalance = value; }

        public override string ToString()
        {
            return $"{id},{name},{branch},{beginningInventory},{stockIn},{stockOut},{totalBalance}"; 
        }
    }
}
