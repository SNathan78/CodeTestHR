
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
 interface IProduct
{
    int Id { get; set; }
    string Name { get; set; }
    int Price { get; set; }
    int ShippingCost { get; set; }
    int Quantity { get; set; }


}
 interface IUser
{
    int Id { get; set; }
    string Name { get; set; }
    decimal Balance { get; set; }
    List<(IProduct product, int quantity)> Orders { get; set; }
}

 interface ICompany
{
    List<IUser> Users { get; set; }
    List<(IProduct product, int quantity)> Products { get; set; }
    void AddProduct(IProduct product, int quantity);
    void AddUser(IUser user);
    void MakeOrder(List<(IProduct product, int quantity)> products, IUser user);
}


class Company : ICompany
    {
        private int highestShippingCost;

        public List<(IProduct product, int quantity)> Products { get; set; }
        public List<IUser> Users { get; set; }
        public Company()
        {
            Products = new List<(IProduct product, int quantity)>();
            Users = new List<IUser>();
        }
        public void MakeOrder(List<(IProduct product, int quantity)> products, IUser user)
        {
            //find highest shippingCost from products and total price of products and check Products have enough quantity decinal highestShippingCost = 0;

            decimal totalPrice = 0;
            decimal highestShippingCost = 0;

        highestShippingCost = products.Max(x => x.product.ShippingCost);

        totalPrice = products.Sum(x => x.Item2 * x.product.Price) + highestShippingCost;

        foreach (var item in products)
        {

            //if (item.Item1.ShippingCost > highestShippingCost)
            //{
            //    highestShippingCost = item.Item1.ShippingCost;
            //}

            //totalPrice += item.Item1.Price + item.Item2;

            var p = Products.FirstOrDefault(x => x.product.Name == item.product.Name);

            if (p.Equals(default((IProduct product, int quantity))))
            {
                //product not found
                return;
            }
            else if (p.quantity < item.quantity)
            {
                //not enough quantity
                return;

            }
            //check if user has enough money

            if (user.Balance < totalPrice + highestShippingCost)
            {
                return;
            }

            user.Balance -= totalPrice; //+ highestShippingCost;
                }

                user.Orders.AddRange(products);

                //decrease Products counts
            
            foreach (var item in products)
            {
                var p = Products.FirstOrDefault(x => x.product.Name == item.product.Name); 
            
                 Products.Remove(p);
                
             if (!p.Equals(default((IProduct product, int quantity))))

                {
                    p.quantity -= item.quantity;
                }

                //update product in list

                Products.Add(p);
            }

        }
        public void AddProduct(IProduct product, int quantity)

        {
            //check if product exist
            var p = Products.FirstOrDefault(x => x.product.Name == product.Name);
            //increase quantity if product exist
            if (!p.Equals(default((IProduct product, int quantity))))

            {
                p.quantity += quantity;
            }
            else
            {
                Products.Add((product, quantity));
            }
        }
        public void AddUser(IUser user)
        {
            Users.Add(user);
        }
    }

    class Product : IProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int ShippingCost { get; set; }
        public int Quantity { get; set; }
        public Product(int id, string name, int price, int shippingCost)
        {
            Id = id;
            Name = name;
            Price = price;
            ShippingCost = shippingCost;
            
        }
    }

    class User : IUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public List<(IProduct product, int quantity)> Orders { get; set; }
        public User(int id, string name, decimal balance)
        {
            Id = id;
            Name = name;
            Balance = balance;

            Orders = new List<(IProduct product, int quantity)>();
        }

    }


class Solution
{
    public static void Main(string[] args)
    {
        //Main is altered from actual provided to make more user friendly and intuitive 

        TextWriter textWriter = new StreamWriter(@"E:\\Output\\Test.txt", true);
        var company = new Company();
        int productCount = 2; //Convert.ToInt32(Console.ReadLine().Trim()); // Hardcoded for just two orders- Refer requirement document
        Console.WriteLine("Enter Product details");
        for (int i = 0; i < productCount; i++)
        {
            var a = Console.ReadLine()?.Trim().Split(',');
            company.AddProduct(new Product(Convert.ToInt32(a[0]), a[1], Convert.ToInt32(a[2]), Convert.ToInt32(a[3])), 
                Convert.ToInt32(a[4]));
        }
        int userCount = 1;// Convert.ToInt32(Console.ReadLine().Trim()); //Hardcoded for just one user
        Console.WriteLine("Enter User Balance Details");
        for (int i = 0; i < userCount; i++)
        {
            var a = Console.ReadLine()?.Trim().Split(',');
            company.AddUser(new User(Convert.ToInt32(a[0]), a[1], Convert.ToInt32(a[2])));
        }

        int orderCount = 1;// Convert.ToInt32(Console.ReadLine().Trim()); //Hardcoded for just one order
        //string order = "1,user1,500";
        Console.WriteLine("Enter Order Details");
        
        for (int i = 0; i < orderCount; i++)
        {
            var a = Console.ReadLine()?.Trim().Split(',');
            var u = company.Users.FirstOrDefault(x => x.Id == Convert.ToInt32(a[0]));
            if (u == null)
            {
                throw new Exception("User not found");
            }



            var orderProducts = new List<(IProduct product, int q)>();
            
            for (int j = 1; j < a.Length; j++)
            {
                var b = a[j].Split('|');
                var c = (company.Products.SingleOrDefault(x => x.product.Id == Convert.ToInt32(b[0])).product, Convert.ToInt32(b[1]));
                if (c.product == null)
                    throw new Exception("Product not found");

                orderProducts.Add(new(c.product, c.Item2));
            }
            company.MakeOrder(orderProducts, u);
        
        }
        textWriter.WriteLine(string.Join("\n", company.Products.OrderBy(x => x.product.Id).Select(x => x.product.
Name + ":" + x.quantity).ToList()));
        textWriter.Flush(); 
        textWriter.Close();

    }
}