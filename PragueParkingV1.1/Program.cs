using System;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace PragueParkingVersionVG
{

    class Program
    {
        public static void Main(string[] args)
        {
            // Ändrar output och input till unicode 
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            // Ändrar konsolfönstret till största möjliga upplösning
            int height = int.Parse(Console.LargestWindowHeight.ToString());
            int width = int.Parse(Console.LargestWindowWidth.ToString());
            Console.SetWindowSize(width, height);
            // Printar ut nuvarande tid
            Console.WriteLine(DateTime.Now.ToString("HH:mm"));
            // Skapar två arrayer med 101 platser och ger de värderna "EMPTY"
            string[] parking = new string[101];
            string[] timeStamp = new string[101];
            Array.Fill(parking, "EMPTY");
            Array.Fill(timeStamp, "EMPTY");

            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu(parking, timeStamp);

            }
            Console.ReadKey(true);
        }



        // Metod för switch case menu med alternativ för olika handlingar 
        public static bool MainMenu(string[] parking, string[] timeStamp)
        {

            {
                // Ändrar färg på texten till mörkgul
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Clear();
                Console.WriteLine(string.Format("{0:HH:mm}", DateTime.Now));
                Console.WriteLine("Hi! And Welcome to Prague parking assistance!");
                Console.WriteLine("How can i be of service?");
                Console.WriteLine("1) Park a vehicle");
                Console.WriteLine("2) Relocate vehicles");
                Console.WriteLine("3) Search for a vehicle by registration number");
                Console.WriteLine("4) Remove vehicle");
                Console.WriteLine("Q) Quit");
                Console.WriteLine("5) View Parking");
                Console.WriteLine("Make a selection:");

                switch (Console.ReadLine().ToLower())
                {
                    case "1":
                        {
                            ParkVehicle(parking, timeStamp);
                            return true;
                        }
                    case "2":
                        {
                            RelocateVehicle(parking, timeStamp);
                            return true;
                        }
                    case "3":
                        {
                            SearchVehicle(parking);
                            return true;
                        }
                    case "4":
                        {
                            ExitParking(parking, timeStamp);
                            return true;
                        }
                    case "q":
                        {
                            Console.WriteLine("Thanks for using Prague parking assistance");
                            Console.WriteLine("Have a nice day!");
                            Environment.Exit(0);
                            return false;
                        }
                    case "5":
                        {
                            PrintArray(parking);
                            return true;
                        }
                    default:
                        {
                            Console.WriteLine("Error: Invalid Input");
                            return true;
                        }
                }
            }
        }

        // Metod för att leta efter parkerat fordon med hjälp av registeringsnummer
        public static void SearchVehicle(string[] parking)
        {
            // Frågar användaren om det är bil eller mc och läser av input och går till motsvarnde case
            Console.WriteLine("Car or MC");
            switch (Console.ReadLine().ToUpper())
            {
                case "CAR":
                    {
                        // Frågar efter registeringsnummber och ger det värdet till en string och om 
                        // parking innehåller hit så skriver den ut vart den är placerad
                        Console.WriteLine("Type the registration number: ");
                        string hit = "CAR" + "_" + Console.ReadLine().ToUpper();
                        for (int i = 1; i < parking.Length - 1; i++)
                        {
                            if (parking[i].Contains(hit))
                            {
                                Console.WriteLine("{0} is located at {1}", hit, i);
                            }
                        }
                        Console.ReadKey();
                        return;
                    }
                case "MC":
                    {
                        // Frågar efter registeringsnummber och ger det värdet till en string och om 
                        // parking innehåller hit så skriver den ut vart den är placerad
                        Console.WriteLine("Type the registration number: ");
                        string hit = "MC" + "_" + Console.ReadLine().ToUpper();
                        for (int i = 100; i > 1; i--)
                        {
                            if (hit == parking[i])
                            {
                                continue;
                            }
                            if (parking[i].Contains('#'))
                            {
                                Console.WriteLine("{0} is located at {1}", hit, i);
                                break;
                            }

                        }
                        Console.ReadKey();
                        return;
                    }

                default: // Om annat än mc eller car skrivs in blir output "Invalid Input!" och går tillbaka till huvudmenyn
                    {
                        Console.WriteLine("Invalid Input!");
                        return;
                    }


            }

        }

        // Metod för flytta på ett parkerat fordon
        public static void RelocateVehicle(string[] parking, string[] timeStamp)
        {
            // Frågar användaren om det är bil eller mc och läser av input och går till motsvarande case
            Console.WriteLine("Car or MC");
            switch (Console.ReadLine().ToUpper())
            {
                case "CAR":
                    {
                        Console.WriteLine("Type in the registration number: ");
                        string hit = "CAR" + "_" + Console.ReadLine().ToUpper();
                        for (int i = 1; i < parking.Length; i++)
                        {

                            if (parking[i].Contains(hit) && parking[i].Contains("MC_"))
                            {
                                Console.WriteLine("Invalid Input! Press any key to continue...");
                                Console.ReadKey();
                                break;
                            }
                            if (parking[i].Contains(hit) && !parking[i].Contains("MC_"))
                            {

                                int tempo = timeStamp[i].IndexOf('%');
                                timeStamp[i] = timeStamp[i].Remove(tempo);
                                hit = hit + "(" + timeStamp[i] + ")" + '%';
                                if (parking[i].Contains(hit) && parking[i].Contains(timeStamp[i]))
                                {
                                    Console.WriteLine("{0} is located at {1}", hit, i);
                                }
                                Console.WriteLine("Do you wish to relocate? (y/n)");
                                string answer = Console.ReadLine().ToUpper();
                                string yes = "Y";
                                string no = "N";
                                if (answer == yes)
                                {
                                    Console.WriteLine("Enter a parkingspot: (1-100)");
                                    string relocate = Console.ReadLine();
                                    IsDigitsOnly(relocate);
                                    int index = int.Parse(relocate);
                                    if (!IsDigitsOnly(relocate))
                                    {
                                        Console.WriteLine("Invalid Input. Numbers only");
                                        Console.ReadKey();
                                        break;
                                    }
                                    if (!parking[index].Contains("EMPTY"))
                                    {
                                        Console.WriteLine("Spot taken, Press any key to continue...");
                                        Console.ReadKey();
                                        break;
                                    }
                                    var buffer = parking[i];
                                    parking[i] = parking[index];
                                    parking[index] = buffer;
                                    var timeMover = timeStamp[i];
                                    timeStamp[index] = timeMover + '%';
                                    Console.WriteLine("Car: {0}, moved to spot : {1}", hit, index);
                                    Console.ReadKey();
                                }
                                while (answer == no)
                                {
                                    break;
                                }
                                break;
                            }

                        }
                        break;
                    }
                case "MC":
                    {
                        Console.WriteLine("Type the registration number: ");
                        string hit = "MC" + "_" + Console.ReadLine().ToUpper();
                        for (int i = 100; i < parking.Length; i--)
                        {
                            if (parking[i].Contains(hit))
                            {
                                Console.WriteLine("{0} is located at {1}", hit, i);
                                Console.WriteLine("Do you wish to relocate? (y/n)");
                                string answer = Console.ReadLine().ToUpper();
                                string yes = "Y";
                                string no = "N";
                                if (answer == yes)
                                {
                                    Console.WriteLine("Enter a Parkingspot: (1-100)");
                                    string relocate = Console.ReadLine();
                                    IsDigitsOnly(relocate);
                                    if (!IsDigitsOnly(relocate))
                                    {
                                        Console.WriteLine("Invalid Input. Numbers only");
                                        Console.ReadKey();
                                        break;
                                    }
                                    int index = int.Parse(relocate);
                                    string[] tempTime = new string[2];
                                    int firstSub = timeStamp[i].IndexOf('#', 0);
                                    tempTime[0] = timeStamp[i].Substring(0, firstSub);
                                    if (parking[i].Contains(hit + '(' + tempTime[0] + ')' + '#'))
                                    {

                                        timeStamp[index] = tempTime[0] + '#';
                                        timeStamp[i] = timeStamp[i].Remove(0, firstSub + 1);
                                        hit += '(' + tempTime[0] + ')' + '#';
                                    }
                                    if (parking[i].Contains('%'))
                                    {
                                        tempTime[1] = timeStamp[i].Substring(firstSub + 1, 5);


                                    }
                                    if (parking[i].Contains(hit + '(' + tempTime[1] + ')' + '%'))
                                    {
                                        timeStamp[i] = timeStamp[i].Remove(firstSub + 1, 6);
                                        if (timeStamp[index].Contains('#'))
                                        {
                                            timeStamp[index] += tempTime[1] + '%';
                                        }
                                        else
                                        {
                                            timeStamp[index] = tempTime[1] + '#';
                                        }
                                        hit += '(' + tempTime[1] + ')' + '%';
                                    }
                                    if (parking[i].Contains(hit) && tempTime[1] == null)
                                    {
                                        parking[i] = "EMPTY";
                                        timeStamp[i] = "EMPTY";

                                    }
                                    else if (parking[i].Contains(hit) && tempTime[1] != null)
                                    {
                                        var IndexRemover = hit.IndexOf('%', 0);
                                        parking[i] = parking[i].Remove(IndexRemover);
                                        parking[i] = parking[i] + '#';
                                        parking[i] = parking[i].Replace('%', '#');
                                    }
                                    if (parking[index].Contains('%'))
                                    {
                                        Console.WriteLine("Spot taken, Press any key to continue...");
                                        Console.ReadKey();
                                        break;
                                    }
                                    if (parking[index].Contains("EMPTY"))
                                    {
                                        parking[index] = String.Empty;
                                        hit = hit.Replace('%', '#');
                                        parking[index] = hit;
                                    }
                                    else if (parking[index].Contains('#'))
                                    {
                                        Console.WriteLine("Do you want to park beside {0}? (y/n)", parking[index]);
                                        answer = Console.ReadLine().ToUpper();

                                        if (answer == yes)
                                        {
                                            hit = hit.Replace('#', '%');
                                            parking[index] += string.Join('#', hit);

                                            if (parking[i] == "")
                                            {
                                                parking[i] = "EMPTY";
                                            }
                                        }
                                        while (answer == no)
                                        {
                                            Console.WriteLine("Too bad. Press any key to continue...");
                                            Console.ReadKey();
                                            break;
                                        }

                                    }
                                    Console.WriteLine("MC: {0}, Successfully moved to spot : {1}", hit, index);
                                    Console.ReadKey();
                                }
                                break;
                            }


                        }
                        break;


                    }


            }
        }

        // Metod för att parkera ett fordon 
        public static void ParkVehicle(string[] parking, string[] timeStamp)
        {
            // Ger string car värdet car och string mc värdet mc
            // Frågar användaren om typ av fordon
            string car = "car".ToUpper();
            string mc = "mc".ToUpper();
            Console.WriteLine("What type of vehicle do you want to park?");
            switch (Console.ReadLine().ToLower()) //  switch case som skickar användaren beroende på typ av fordon 
            {
                case "car":
                    {
                        ParkingCar(car, parking, timeStamp);
                        DoneParking(parking, timeStamp);
                        break;
                    }
                case "mc":
                    {
                        ParkingMC(mc, parking, timeStamp);
                        DoneParking(parking, timeStamp);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        // Metod för att registera car i arrayen
        public static void ParkingCar(string car, string[] parking, string[] timeStamp)
        {
            // Ber användaren att skriva reg nummer och ger den värdet till string regNumberCar och 
            // ger invalid input om den överskrider 10 tecken och skickar tillbaka den till input regnummer
            Console.WriteLine("Enter registration number:");
            string regNumberCar = Console.ReadLine().ToUpper();
            while (!regNumberCar.Contains("CAR_"))
            {
                if (regNumberCar.Length <= 10)
                {
                    regNumberCar = "CAR" + '_' + regNumberCar;
                }
                else if (regNumberCar.Length > 10)
                {
                    Console.WriteLine("Too many Characters, Please try again (Max 10)");
                    break;
                }

                // Går igenom hela arrayen och ger nuvarande tid som värde till string currentTime
                for (int i = 1; i < parking.Length; i++)
                {
                    string currentTime = DateTime.Now.ToString("HH:mm");
                    if (parking[i].Contains('%'))
                    {
                        continue;
                    }
                    if (parking[i].Contains("EMPTY"))
                    {
                        timeStamp[i] = currentTime;
                        parking[i] = regNumberCar + '(' + timeStamp[i] + ')' + '%';
                        timeStamp[i] += '%';
                        break;
                    }
                }
            }
        }

        // Metod för att registera mc i arrayen
        public static void ParkingMC(string mc, string[] parking, string[] timeStamp)
        {
            // Ber användaren att skriva reg nummer och ger den värdet till string regNumberCar och
            // ger invalid input om den överskrider 10 tecken och skickar tillbaka den till input regnummer
            Console.WriteLine("Enter registration number:");
            string regNumberMc = Console.ReadLine().ToUpper();
            while (!regNumberMc.Contains("MC_"))
            {
                if (regNumberMc.Length <= 10)
                {
                    regNumberMc = "MC" + '_' + regNumberMc;
                }
                else if (regNumberMc.Length > 10)
                {
                    Console.WriteLine("Too many Characters, Please try again (Max 10)");
                    break;
                }

                for (int i = 100; i > 1; i--)
                {
                    string currentTime = DateTime.Now.ToString("HH:mm");
                    if (parking[i].Contains('%'))
                    {
                        continue;
                    }
                    if (parking[i].Contains('#'))
                    {

                        timeStamp[i] += currentTime;
                        parking[i] += regNumberMc + "(" + currentTime + ")" + '%';
                        timeStamp[i] += '%';
                        break;
                    }
                    if (parking[i].Contains("EMPTY"))
                    {
                        timeStamp[i] = currentTime;
                        parking[i] = regNumberMc + "(" + timeStamp[i] + ")" + '#';
                        timeStamp[i] += '#';
                        break;
                    }

                }
            }
        }

        // Metod för att avsluta loop när man har parkerat klart
        public static void DoneParking(string[] parking, string[] timeStamp)
        {
            // Frågar användaren om man gar parkerat klart och 
            // gör output värdet till string "answer" och
            // ger string "yes1" värdet y och "no1" värdet n
            Console.WriteLine("Are you done Parking? (y/n)");
            switch (Console.ReadLine().ToLower())
            {
                case "y":
                    {
                        break;
                    }
                default:
                    {
                        ParkVehicle(parking, timeStamp);
                        break;
                    }
            }
        }

        // Metod för att visa alla parkeringsplatser och se om de är upptagna eller inte
        public static void PrintArray(string[] parking)
        {
            // Rensar kosnolfönstret och delar upp arrayen så den är splittad i 5 kolumner och ger platsen färgen gul 
            // och "EMPTY" grön om platsen är ledig annars röd med fordon typen och reg nummer 
            Console.Clear();
            int y = 0;
            for (int j = 1; j < 21; j++)
            {

                Console.SetCursorPosition(0, y);
                if (parking[j].Contains("EMPTY"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("{0}:", j);
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" {0}", parking[j]);
                    Console.ResetColor();

                }
                if (!parking[j].Contains("EMPTY"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("{0}:", j);
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(" {0}", parking[j]);
                    Console.ResetColor();
                }
                y++;
            }
            int q = 0;
            for (int j = 21; j < 41; j++)
            {
                Console.SetCursorPosition(40, q);
                if (parking[j].Contains("EMPTY"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("{0}:", j);
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" {0}", parking[j]);
                    Console.ResetColor();
                }
                if (!parking[j].Contains("EMPTY"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("{0}:", j);
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(" {0}", parking[j]);
                    Console.ResetColor();
                }
                q++;
            }
            int d = 0;
            for (int j = 41; j < 61; j++)
            {
                Console.SetCursorPosition(80, d);
                if (parking[j].Contains("EMPTY"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("{0}:", j);
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" {0}", parking[j]);
                    Console.ResetColor();
                }
                if (!parking[j].Contains("EMPTY"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("{0}:", j);
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(" {0}", parking[j]);
                    Console.ResetColor();
                }
                d++;
            }
            int z = 0;
            for (int j = 61; j < 81; j++)
            {
                Console.SetCursorPosition(120, z);
                if (parking[j].Contains("EMPTY"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("{0}:", j);
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" {0}", parking[j]);
                    Console.ResetColor();
                }
                if (!parking[j].Contains("EMPTY"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("{0}:", j);
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(" {0}", parking[j]);
                    Console.ResetColor();
                }
                z++;
            }
            int w = 0;
            for (int j = 81; j < 101; j++)
            {
                Console.SetCursorPosition(160, w);
                if (parking[j].Contains("EMPTY"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("{0}:", j);
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" {0}", parking[j]);
                    Console.ResetColor();
                }
                if (!parking[j].Contains("EMPTY"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("{0}:", j);
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(" {0}", parking[j]);
                    Console.ResetColor();
                }
                w++;




            }

            // Printar ut text med färgen blå till main menu 
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(0, 26);
            Console.WriteLine("Press enter to get back..."); Console.ReadKey();

        }

        // 
        private static bool IsDigitsOnly(string relocate)
        {
            foreach (char c in relocate)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
        // Metod för att ta bort parkerat fordon 
        public static void ExitParking(string[] parking, string[] timeStamp)
        {
            {
                // Frågar användaren om det är Car eller Mc och skickar vidare till den case som motsvaras
                Console.WriteLine("Car or MC");
                switch (Console.ReadLine().ToUpper())
                {
                    case "CAR":
                        {
                            // Ber användaren skriva in reg nummer och ger string hit värdet med car och _

                            Console.WriteLine("Type the registration number: ");
                            string hit = "CAR" + "_" + Console.ReadLine().ToUpper();
                            for (int i = 1; i < parking.Length; i++)
                            {    // Om hit inte matchar med element i parking blir output "Invalid input" och bryts  
                                if (parking[i].Contains(hit) && parking[i].Contains("MC_"))
                                {
                                    Console.WriteLine("Invalid Input! Press any key to continue...");
                                    Console.ReadKey();
                                    break;
                                }
                                // Om hit matchar med element i parking blir output fordon + reg nummer och var den är inom parkering
                                if (parking[i].Contains(hit))
                                {
                                    Console.WriteLine("{0} is located at {1}", hit, i);
                                }
                                Console.WriteLine("Do you want to leave the parking? (y/n)");
                                string answer = Console.ReadLine().ToLower();
                                string y = "y";
                                string n = "n";
                                if (answer == y)
                                {
                                    int hours = 0;
                                    parking[i] = String.Empty;
                                    parking[i] = "EMPTY";
                                    string receipt = timeStamp[i];
                                    receipt = Regex.Replace(receipt, @"[^0-9a-zA-Z]+", "");
                                    int ReceiptTwo = int.Parse(receipt);
                                    string currentTime = DateTime.Now.ToString("HH:mm");
                                    currentTime = Regex.Replace(currentTime, @"[^0-9a-zA-Z]+", "");
                                    int timeConverter = int.Parse(currentTime);
                                    int minutes = timeConverter - ReceiptTwo;

                                    while (minutes >= 60)
                                    {
                                        hours++;
                                        minutes -= 60;
                                    }
                                    timeStamp[i] = "EMPTY";
                                    Console.WriteLine("You parked here for: {0} Hours and {1} Minutes", hours, minutes);
                                    Console.WriteLine("Please come again!");
                                    Console.ReadKey();
                                    break;
                                }
                                if (answer == n)
                                {
                                    Console.WriteLine("Action cancelled, Press any key to continue...");
                                    break;
                                }
                            }
                            return;
                        }
                    case "MC":
                        {
                            string y = "y";
                            string n = "n";
                            int hours = 0;
                            string receipt = String.Empty;
                            // Ber användaren skriva in reg nummer och ger string hit värdet med mc och _
                            Console.WriteLine("Type the registration number: ");
                            string hit = "MC" + "_" + Console.ReadLine().ToUpper();
                            for (int i = 100; i < parking.Length; i--)
                            {
                                // Om hit matchar med element i parking blir output fordon + reg nummer och var den är inom parkering
                                if (parking[i].Contains(hit))
                                {
                                    Console.WriteLine("{0} is located at {1}", hit, i);
                                }
                                if (!parking[i].Contains(hit))
                                {

                                    continue;
                                }
                                // Om hit inte matchar med element i parking blir output "Invalid input" och bryts  
                                else if (i == 1 && !parking[i].Contains(hit))
                                {
                                    Console.WriteLine("Invalid Input. Vehicle does not exist, Press any key to continue...");
                                    Console.ReadKey();
                                    break;
                                }
                                Console.WriteLine("Do you want to leave the parking? (y/n)");
                                string answer = Console.ReadLine().ToLower();

                                if (answer == y)
                                {
                                    string[] tempTime = new string[2];
                                    int firstSub = timeStamp[i].IndexOf('#', 0);
                                    int secondSub = parking[i].IndexOf('#', 0);
                                    if (!parking[i].Contains('%'))
                                    {
                                        tempTime[0] = timeStamp[i].Substring(0, firstSub);
                                    }
                                    else if (parking[i].Contains('%'))
                                    {
                                        tempTime[0] = timeStamp[i].Substring(0, firstSub);
                                        tempTime[1] = timeStamp[i].Substring(firstSub + 1, 5);
                                    }
                                    if (parking[i].Contains(hit + '(' + tempTime[0] + ')' + '#') && tempTime[1] == null)
                                    {
                                        hit += '(' + tempTime[0] + ')' + '#';
                                        receipt = tempTime[0];
                                        parking[i] = "EMPTY";
                                        timeStamp[i] = "EMPTY";
                                    }
                                    else if (parking[i].Contains(hit + '(' + tempTime[0] + ')' + '#') && tempTime[1] != null)
                                    {
                                        hit += '(' + tempTime[0] + ')' + '#';
                                        timeStamp[i] = timeStamp[i].Remove(0, firstSub + 1);
                                        parking[i] = parking[i].Remove(0, hit.Length);
                                        timeStamp[i] = timeStamp[i].Replace('%', '#');
                                        receipt = tempTime[0];
                                        parking[i] = parking[i].Replace('%', '#');
                                    }
                                    else if (parking[i].Contains(hit + '(' + tempTime[1] + ')' + '%'))
                                    {
                                        hit += '(' + tempTime[1] + ')' + '%';
                                        receipt = tempTime[1];
                                        parking[i] = parking[i].Remove(secondSub, hit.Length);
                                        parking[i] = parking[i].Replace('%', '#');
                                        timeStamp[i] = timeStamp[i].Remove(firstSub + 1, 6);

                                    }
                                    receipt = Regex.Replace(receipt, @"[^0-9a-zA-Z]+", "");
                                    int receiptTwo = int.Parse(receipt);
                                    string currentTime = DateTime.Now.ToString("HH:mm");
                                    currentTime = Regex.Replace(currentTime, @"[^0-9a-zA-Z]+", "");
                                    int timeConverter = int.Parse(currentTime);
                                    int minutes = timeConverter - receiptTwo;
                                    while (minutes >= 60)
                                    {
                                        hours++;
                                        minutes -= 60;
                                    }
                                    Console.WriteLine("You parked here for: {0} Hours and {1} Minutes", hours, minutes);
                                    Console.WriteLine("Please come again!");
                                    Console.ReadKey();
                                    break;
                                }
                                if (answer == n)
                                {
                                    Console.WriteLine("Action cancelled, Press any key to continue...");
                                    break;
                                }
                            }
                            return;
                        }

                    default:
                        {
                            Console.WriteLine("Invalid Input!");
                            return;
                        }


                }

            }
        }
    }
}
