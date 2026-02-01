namespace BlockWebsites
{
    internal class Program
    {
        static void ShowMenu() {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Blocked Sites: {FileManager.GetListSitesCount}");
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║               WEB BLOCKER              ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine(" 1. [Add]    Block new sites");
            Console.WriteLine(" 2. [Check]  View current list");
            Console.WriteLine(" 3. [Delete] Remove a site");
            Console.WriteLine(" 4. [Apply]  Sync with System (Hosts)");
            Console.WriteLine(" 5. [Exit]   Close application");
            Console.WriteLine("──────────────────────────────────────────");
            Console.Write(" Select an option [1-5]: ");
        }

        static void Main(string[] args) {
            Console.Title = "WebBlocker CLI";

            FileManager.CreateDataFile();
            FileManager.ReadDataFile(SystemPaths.DataFilePath);

            while (true)
            {
                Console.Clear();
                ShowMenu();
                string userChoice = Console.ReadLine()!;
                Console.ResetColor();

                switch (userChoice)
                {
                    case "1":
                        FileManager.AddOneOrMultipleSitesToBlock();
                        break;
                    case "2":
                        FileManager.DisplayBlockedSites();
                        break;
                    case "3":
                        FileManager.DeleteSites();
                        break;
                    case "4":
                        Console.WriteLine("\nApplying changes to Hosts file...");
                        FileManager.SyncWithHosts();
                        break;
                    case "5":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nClosing... Goodbye!");
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInvalid option. Please try again.");
                        break;
                }
                Console.WriteLine("\n──────────────────────────────────────────");
                Console.WriteLine("Action completed. Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
