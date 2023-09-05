using System;
using System.Collections.Generic;
using System.Linq;

class Card
{
    public string PAN { get; set; }
    public string PIN { get; set; }
    public string CVC { get; set; }
    public string ExpireDate { get; set; }
    public decimal Balance { get; set; }
}

class User
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public Card CreditCard { get; set; }
}

class Program
{
    static List<User> users = new List<User>();

    static void Main(string[] args)
    {
        // Fake data
        CreateUser("John", "Doe", "1234567890123456", "1234", "123", "06/23", 1000.00M);
        CreateUser("Alice", "Smith", "9876543210123456", "5678", "456", "09/24", 2000.00M);
        CreateUser("Bob", "Johnson", "1111222233334444", "4321", "789", "12/25", 1500.00M);
        CreateUser("Eve", "Davis", "4444333322221111", "8765", "321", "03/26", 3000.00M);
        CreateUser("Charlie", "Brown", "5555666677778888", "9876", "654", "06/27", 2500.00M);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Pin kodunuzu daxil edin:");
            string enteredPIN = Console.ReadLine();

            User user = AuthenticateUser(enteredPIN);

            if (user != null)
            {
                Console.WriteLine($"{user.Name} {user.Surname}, xoş geldiniz. Emeliyyat secin:");
                Console.WriteLine("1. Kart Balansini goster");
                Console.WriteLine("2. Nagd pul cekmek");
                Console.WriteLine("3. Emeliyyat kecmisi");
                Console.WriteLine("4. Kartdan Karta Kocurme");
                Console.WriteLine("5. Exit");

                int choice = GetMenuChoice(1, 5);

                switch (choice)
                {
                    case 1:
                        Console.WriteLine($"Kart Balansiniz: {user.CreditCard.Balance:C2}");
                        break;
                    case 2:
                        WithdrawCash(user);
                        break;
                    case 3:
                        ShowTransactionHistory(user);
                        break;
                    case 4:
                        TransferToAnotherCard(user);
                        break;
                    case 5:
                        Environment.Exit(0);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Bu PIN koda ait kart tapilmadi. Yeniden cehd edin");
            }

            Console.WriteLine("Davam etmek ucun bir duymeye basin...");
            Console.ReadKey();
        }
    }

    static void CreateUser(string name, string surname, string pan, string pin, string cvc, string expireDate, decimal balance)
    {
        var card = new Card
        {
            PAN = pan,
            PIN = pin,
            CVC = cvc,
            ExpireDate = expireDate,
            Balance = balance
        };

        var user = new User
        {
            Name = name,
            Surname = surname,
            CreditCard = card
        };

        users.Add(user);
    }

    static User AuthenticateUser(string enteredPIN)
    {
        return users.FirstOrDefault(user => user.CreditCard.PIN == enteredPIN);
    }

    static void WithdrawCash(User user)
    {
        Console.WriteLine("Cekmek istediyiniz meblegi daxil edin:");
        Console.WriteLine("1. 10 AZN");
        Console.WriteLine("2. 20 AZN");
        Console.WriteLine("3. 50 AZN");
        Console.WriteLine("4. 100 AZN");
        Console.WriteLine("5. Diger");

        int choice = GetMenuChoice(1, 5);

        decimal amountToWithdraw = choice switch
        {
            1 => 10.00M,
            2 => 20.00M,
            3 => 50.00M,
            4 => 100.00M,
            _ => GetCustomWithdrawalAmount()
        };

        if (amountToWithdraw > user.CreditCard.Balance)
        {
            Console.WriteLine("Movcud olmayan mebleg. Emeliyyat icra olunmadi :/");
        }
        else
        {
            user.CreditCard.Balance -= amountToWithdraw;
            Console.WriteLine($"{amountToWithdraw:C2} cekildi. Yeni mebleg: {user.CreditCard.Balance:C2}");
        }
    }

    static decimal GetCustomWithdrawalAmount()
    {
        Console.Write("Cekmek istediyiniz meblegi daxil edin: ");
        decimal customAmount = decimal.Parse(Console.ReadLine());

        if (customAmount <= 0)
        {
            Console.WriteLine("Bele bir mebleg yoxdur. Var olan meblege uyğun daxil edin.");
            return GetCustomWithdrawalAmount();
        }

        return customAmount;
    }

    static void ShowTransactionHistory(User user)
    {
        Console.WriteLine("Emeliyyat kecmisi:");

    }

    static void TransferToAnotherCard(User user)
    {
        Console.WriteLine("Kartdan Karta kocurme emeliyyati icra edildi.");
        Console.WriteLine("Hedef kartın PIN kodunu daxil edin:");

        string targetPIN = Console.ReadLine();
        User targetUser = AuthenticateUser(targetPIN);

        if (targetUser != null)
        {
            Console.WriteLine($"Kocurmek istediyiniz meblegi daxil edin (Movcud mebleg: {user.CreditCard.Balance:C2}):");
            decimal transferAmount = decimal.Parse(Console.ReadLine());

            if (transferAmount <= 0 || transferAmount > user.CreditCard.Balance)
            {
                Console.WriteLine("Bele bir mebleg yoxdu. Emeliyyat icra olunmadi :/");
            }
            else
            {
                user.CreditCard.Balance -= transferAmount;
                targetUser.CreditCard.Balance += transferAmount;

                Console.WriteLine($"{transferAmount:C2} kocuruldu.");
            }
        }
        else
        {
            Console.WriteLine("Hedef kart tapilmadi. Emeliyyat icra olunmadi :/");
        }
    }

    static int GetMenuChoice(int min, int max)
    {
        int choice;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                if (choice >= min && choice <= max)
                {
                    return choice;
                }
            }
            Console.WriteLine("Bele bir secim yoxdur. Yeniden cehd edin");
        }
    }
}
