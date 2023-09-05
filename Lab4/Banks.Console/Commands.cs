using System.Collections.ObjectModel;

namespace Banks.Console;

public class Commands
{
    public const int AvailableDepositSumCount = 3;
    public const int MinimalSum = 0;
    public const decimal MinimalPercent = 0;
    public const int MaximalDifferenceBetweenDepositAccountSums = 100000;
    public const decimal MaximalDifferenceBetweenDepositAccountPercents = 3;
    public const int MinimalCreditLimitSum = 100000;
    public const int MaximalCreditLimitSum = 500000;
    public const int MinimalDubiousSum = 5000;
    public const int MaximalDubiousSum = 10000;
    public const decimal MaximalDebitAccountPercent = 15;
    public const decimal MaximalCreditCommissionPercent = 8;
    public const decimal MaximalTransferCommissionPercent = 5;
    public const int MaximalDepositAccountMinDurationInDays = 730;
    public const int MinimalDepositAccountMinDurationInDays = 30;
    public const int MinimalDaysAmount = 1;
    public const int MinimalId = 0;
    public const int BankErrorNumber = -1;
    public const int ClientErrorNumber = -1;
    public const int AccountErrorNumber = -1;
    private CentralBank _centralBank;

    private Dictionary<string, Execute> _commands;

    public Commands()
    {
        _centralBank = CentralBank.GetCentralBankInstance();
        _commands = new Dictionary<string, Execute>
        {
            { "help", Help },
            { "create_bank", CreateBank },
            { "accrue_percents", AccruePercentsAndCommissions },
            { "add_client", AddClient },
            { "open_debit_account", OpenDebitAccount },
            { "open_deposit_account", OpenDepositAccount },
            { "open_credit_account", OpenCreditAccount },
            { "take_off", TakeOff },
            { "top_up", TopUp },
            { "transfer", Transfer },
            { "cancel_transaction", CancelTransaction },
            { "show_banks", ShowBanks },
            { "show_clients_of_bank", ShowClientsOfBank },
            { "show_client_accounts", ShowClientAccounts },
            { "clear", Clear },
        };
    }

    public delegate void Execute();

    public void AddCommand(string command, Execute executeCommand)
    {
        _commands.Add(command, executeCommand);
    }

    public void ExecuteCommand(string? command)
    {
        if (CheckCommand(command) == false)
        {
            System.Console.WriteLine("No such command!\n Use help to see all available commands");
            return;
        }

        if (command == null)
            return;

        _commands[command].Invoke();
    }

    private void ShowBanks()
    {
        int count = 0;
        System.Console.WriteLine("Id:       Titles:");
        foreach (Bank bank in _centralBank.Banks)
        {
            System.Console.WriteLine($"{count}) {bank.Title}");
        }
    }

    private void Clear()
    {
        System.Console.Clear();
    }

    private void ShowClientAccounts()
    {
        Client? client = FindClient(out Bank? bank);
        if (client == null || bank == null)
            return;
        List<Account> accounts = bank.GetClientAccounts(client);
        System.Console.WriteLine("AccountID:    AccountType:    AccountSum:");
        foreach (Account account in accounts)
        {
            System.Console.WriteLine($"{account.Id}     {account.GetType()}     {account.Sum}");
        }
    }

    private void ShowClientsOfBank()
    {
        Bank? bank = FindBank();
        if (bank == null)
            return;
        int count = 1;
        System.Console.WriteLine("Client:    Name:    Surname:     Address:     " +
                                 "PassportNumber:     Notifications:     Id:");
        foreach (Client client in bank.Clients)
        {
            System.Console.WriteLine($"{count}) {client.Name} {client.Surname} {client.Address} " +
                                     $"{client.PassportNumber} {client.GetCommonNotifications} {client.Id}");
        }
    }

    private void Help()
    {
        System.Console.WriteLine("Available operations:");
        int count = 1;
        foreach (string command in _commands.Keys)
        {
            System.Console.WriteLine($"{count}) {command}");
            count++;
        }
    }

    private void CancelTransaction()
    {
        System.Console.WriteLine("CANCELLING TRANSACTION:");
        Account? account1 = FindAccount(out Bank? bank1);
        if (account1 == null || bank1 == null)
            return;
        Account? account2 = FindAccount(out Bank? bank2);
        if (account2 == null || bank2 == null)
            return;
        Sum:
        decimal sum = DefineSumToTransfer();
        if (sum < MinimalSum)
        {
            System.Console.WriteLine($"Sum to transfer must be > {MinimalSum}\n" +
                                     $"try again :)");
            goto Sum;
        }

        System.Console.WriteLine("Canceling transfer...");
        try
        {
            bank1.CancelTransaction(bank2, account1, account2, sum);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }

        System.Console.WriteLine("Transfer canceled!");
    }

    private void Transfer()
    {
        System.Console.WriteLine("TRANSFERRING MONEY TO ACCOUNT:");
        Account? account1 = FindAccount(out Bank? bank1);
        if (account1 == null || bank1 == null)
            return;
        Account? account2 = FindAccount(out Bank? bank2);
        if (account2 == null || bank2 == null)
            return;
        Sum:
        decimal sum = DefineSumToTransfer();
        if (sum < MinimalSum)
        {
            System.Console.WriteLine($"Sum to transfer must be > {MinimalSum}\n" +
                                     $"try again :)");
            goto Sum;
        }

        System.Console.WriteLine("Transferring...");

        try
        {
            bank1.TransferMoneyToAnotherAccount(bank2, account1, account2, sum);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }

        System.Console.WriteLine("Transferred!");
    }

    private void TopUp()
    {
        System.Console.WriteLine("TOPPING-UP MONEY TO ACCOUNT:");
        Account? account = FindAccount(out Bank? bank);
        if (account == null)
            return;
        Sum:
        decimal sum = DefineSumToTopUp();
        if (sum < MinimalSum)
        {
            System.Console.WriteLine($"Sum to top-up must be > {MinimalSum}\n" +
                                     $"try again :)");
            goto Sum;
        }

        System.Console.WriteLine("Topping up sum...");
        try
        {
            account.TopUp(sum);
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Sum topped up!");
    }

    private void TakeOff()
    {
        System.Console.WriteLine("TAKING OFF MONEY FROM ACCOUNT:");
        Account? account = FindAccount(out Bank? bank);
        if (account == null)
            return;
        Sum:
        decimal sum = DefineSumToTakeoff();
        if (sum < MinimalSum)
        {
            System.Console.WriteLine($"Sum to take off must be > {MinimalSum}\n" +
                                     $"try again :)");
            goto Sum;
        }

        System.Console.WriteLine("Taking off sum...");
        try
        {
            account.TakeOff(sum);
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Sum took off!");
    }

    private void OpenCreditAccount()
    {
        System.Console.WriteLine("CREDIT ACCOUNT OPENING:");
        Client? client = FindClient(out Bank? bank);
        if (client == null || bank == null)
            return;
        decimal sum = DefineCreditAccountSum();
        bool getNotification = DefineNotifications();
        System.Console.WriteLine("Opening credit account...");
        bank.OpenCreditAccount(client, sum, getNotification);
        System.Console.WriteLine("Credit account created!");
    }

    private void OpenDepositAccount()
    {
        System.Console.WriteLine("DEPOSIT ACCOUNT OPENING:");
        Client? client = FindClient(out Bank? bank);
        if (client == null || bank == null)
            return;
        int durationInDays = DefineDurationInDaysForDepositAccount();
        decimal sum = DefineDepositAccountSum();
        bool getNotification = DefineNotifications();
        System.Console.WriteLine("Opening deposit account...");
        bank.OpenDepositAccount(client, durationInDays, sum, getNotification);
        System.Console.WriteLine("Deposit account created!");
    }

    private void OpenDebitAccount()
    {
        System.Console.WriteLine("DEBIT ACCOUNT OPENING:");
        Client? client = FindClient(out Bank? bank);
        if (client == null || bank == null)
            return;
        decimal sum = DefineDebitAccountSum();
        bool getNotification = DefineNotifications();
        System.Console.WriteLine("Opening debit account...");
        bank.OpenDebitAccount(client, sum, getNotification);
        System.Console.WriteLine("Debit account created!");
    }

    private void AddClient()
    {
        System.Console.WriteLine("ADD CLIENT:");
        Bank? bank = FindBank();
        if (bank == null)
            return;

        System.Console.WriteLine("Insert client's data:");
        string name = DefineClientName();
        string surname = DefineClientSurname();
        string? address = DefineAddress();
        string? passportNumber = DefinePassportNumber();
        bool getCommonNotifications = DefineNotifications();
        System.Console.WriteLine("Adding client...");
        bank.AddClient(name, surname, address, passportNumber, getCommonNotifications);
        System.Console.WriteLine("Client added!");
    }

    private void CreateBank()
    {
        System.Console.WriteLine("CREATING BANK:");
        Dictionary<int, decimal> depositPercents = new ();
        string title = DefineBankTitle();
        DefineDepositPercents(depositPercents);
        DefinePercentsAndCommissions(out decimal debitPercent, out decimal creditCommissionPercent, out decimal transferCommission);
        DefineSumsAndLimits(out int creditLimit, out int dubiousSum, out int depositAccountMinDurationInDays);
        System.Console.WriteLine("Creating bank...");
        _centralBank.RegisterBank(depositPercents, debitPercent, creditCommissionPercent, creditLimit, transferCommission, dubiousSum, depositAccountMinDurationInDays, title);
        System.Console.WriteLine("Bank created!");
    }

    private void AccruePercentsAndCommissions()
    {
        System.Console.WriteLine("ACCRUING PERCENTS AND COMMISSIONS:");
        DaysAmount:
        int daysAmount = DefineDaysAmount();
        if (daysAmount < MinimalDaysAmount)
        {
            System.Console.WriteLine($"Days amount must be >= {MinimalDaysAmount} days\n" +
                                     $"try again :)");
            goto DaysAmount;
        }

        _centralBank.AccrueCommissionAndPercents(daysAmount);
        System.Console.WriteLine("Percents and commissions accrued!");
    }

    private void DefinePercentsAndCommissions(out decimal debitPercent, out decimal creditCommissionPercent, out decimal transferCommission)
    {
        DebitPercent:
        debitPercent = DefineDebitAccountPercent();
        if (debitPercent > MaximalDebitAccountPercent || debitPercent < MinimalPercent)
        {
            System.Console.WriteLine($"Debit percent can not be > {MaximalDebitAccountPercent}% and < {MinimalPercent}%\n " +
                                     $"try again :)");
            goto DebitPercent;
        }

        CreditCommissionPercent:
        creditCommissionPercent = DefineCreditCommissionPercent();
        if (creditCommissionPercent > MaximalCreditCommissionPercent || debitPercent < MinimalPercent)
        {
            System.Console.WriteLine($"Credit commission percent can not be > {MaximalCreditCommissionPercent}% and < {MinimalPercent}%\n" +
                                     $"try again :)");
            goto CreditCommissionPercent;
        }

        TransferCommissionPercent:
        transferCommission = DefineTransferCommissionPercent();
        if (transferCommission > MaximalTransferCommissionPercent || debitPercent < MinimalPercent)
        {
            System.Console.WriteLine($"Transfer commission percent can not be > {MaximalTransferCommissionPercent}% and < {MinimalPercent}%\n" +
                                     $"try again :)");
            goto TransferCommissionPercent;
        }
    }

    private void DefineSumsAndLimits(out int creditLimit, out int dubiousSum, out int depositAccountMinDurationInDays)
    {
        CreditLimitSum:
        creditLimit = DefineCreditLimitSum();
        if (creditLimit < MinimalCreditLimitSum || creditLimit > MaximalCreditLimitSum)
        {
            System.Console.WriteLine($"Credit sum must be >= {MinimalCreditLimitSum} and <= {MaximalCreditLimitSum}\n" +
                                     $"try again");
            goto CreditLimitSum;
        }

        DubiousSum:
        dubiousSum = DefineDubiousSum();
        if (dubiousSum < MinimalDubiousSum || dubiousSum > MaximalDubiousSum)
        {
            System.Console.WriteLine($"Dubious sum must be >= {MinimalDubiousSum} and <= {MaximalDubiousSum}\n" +
                                     $"try again :)");
            goto DubiousSum;
        }

        DepositAccountMinDurationInDays:
        depositAccountMinDurationInDays = DefineDepositAccountMinDurationInDays();
        if (depositAccountMinDurationInDays < MinimalDepositAccountMinDurationInDays ||
            depositAccountMinDurationInDays > MaximalDepositAccountMinDurationInDays)
        {
            System.Console.WriteLine($"Deposit account minimal duration in days must be >= {MinimalDepositAccountMinDurationInDays} and <= {MaximalDepositAccountMinDurationInDays}\n" +
                                     $"try again :)");
            goto DepositAccountMinDurationInDays;
        }
    }

    private void DefineDepositPercents(Dictionary<int, decimal> depositPercents)
    {
        System.Console.WriteLine("Define deposit percents:");
        int currentSum = MinimalSum;
        decimal currentPercent = MinimalPercent;
        int i = 1;
        while (true)
        {
            Sum:
            int sum = DefineDepositAccountSums(i);
            if (sum <= currentSum || sum >= currentSum + MaximalDifferenceBetweenDepositAccountSums)
            {
                System.Console.WriteLine(
                    $"Sum can not be <= prev sum and >= prev sum + {MaximalDifferenceBetweenDepositAccountSums}\n" +
                    $"Your prev sum: {currentSum}\n " +
                    $"try again :)");
                goto Sum;
            }

            currentSum = sum;
            Percent:
            decimal percent = DefineDepositAccountPercent(i);
            if (percent <= currentPercent || percent >= currentPercent + MaximalDifferenceBetweenDepositAccountPercents)
            {
                System.Console.WriteLine($"Percent can not be <= prev percent and >= prev percent + {MaximalDifferenceBetweenDepositAccountPercents}\n" +
                                         $"Your prev percent: {currentPercent}\n " +
                                         $"try again :)");
                goto Percent;
            }

            currentPercent = percent;
            depositPercents[sum] = percent;
            i++;
            if (i > AvailableDepositSumCount)
                break;
        }
    }

    private int DefineSum()
    {
        while (true)
        {
            if (int.TryParse(System.Console.ReadLine(), out int sum))
                return sum;
            System.Console.WriteLine("Invalid data format! Try again :)");
        }
    }

    private int DefineDaysAmount()
    {
        System.Console.WriteLine("Insert days amount during which percents and commissions will be accrued:");
        int daysAmount = DefineSum();
        return daysAmount;
    }

    private int DefineDurationInDaysForDepositAccount()
    {
        System.Console.WriteLine("Insert duration in days for deposit account:");
        int duration = DefineSum();
        return duration;
    }

    private int DefineCreditLimitSum()
    {
        System.Console.WriteLine("Insert credit limit sum:");
        int sum = DefineSum();
        return sum;
    }

    private int DefineDepositAccountMinDurationInDays()
    {
        System.Console.WriteLine("Insert deposit account minimal duration in days:");
        int sum = DefineSum();
        return sum;
    }

    private int DefineDubiousSum()
    {
        System.Console.WriteLine("Insert dubious sum:");
        int sum = DefineSum();
        return sum;
    }

    private int DefineDepositAccountSums(int count)
    {
        System.Console.WriteLine($"Insert {count} sum for deposit account:");
        int sum = DefineSum();
        return sum;
    }

    private decimal DefineDepositAccountSum()
    {
        System.Console.WriteLine($"Insert sum for deposit account:");
        decimal sum = DefinePercent();
        return sum;
    }

    private decimal DefinePercent()
    {
        while (true)
        {
            if (decimal.TryParse(System.Console.ReadLine(), out decimal percent))
                return percent;
            System.Console.WriteLine("Invalid data format! Try again :)");
        }
    }

    private decimal DefineSumToTakeoff()
    {
        System.Console.WriteLine("Insert sum to takeoff:");
        decimal sum = DefinePercent();
        return sum;
    }

    private decimal DefineSumToTopUp()
    {
        System.Console.WriteLine("Insert sum to top-up:");
        decimal sum = DefinePercent();
        return sum;
    }

    private decimal DefineSumToTransfer()
    {
        System.Console.WriteLine("Insert sum to transfer:");
        decimal sum = DefinePercent();
        return sum;
    }

    private decimal DefineDebitAccountSum()
    {
        System.Console.WriteLine("Insert debit account sum:");
        decimal sum = DefinePercent();
        return sum;
    }

    private decimal DefineCreditAccountSum()
    {
        System.Console.WriteLine("Insert credit account sum:");
        decimal sum = DefinePercent();
        return sum;
    }

    private decimal DefineDebitAccountPercent()
    {
        System.Console.WriteLine($"Insert percent for debit account:");
        decimal percent = DefinePercent();
        return percent;
    }

    private decimal DefineCreditCommissionPercent()
    {
        System.Console.WriteLine($"Insert percent for credit commission:");
        decimal percent = DefinePercent();
        return percent;
    }

    private decimal DefineTransferCommissionPercent()
    {
        System.Console.WriteLine($"Insert percent for transfer commission:");
        decimal percent = DefinePercent();
        return percent;
    }

    private decimal DefineDepositAccountPercent(int count)
    {
        System.Console.WriteLine($"Insert percent for the {count} sum of deposit account:");
        decimal percent = DefinePercent();
        return percent;
    }

    private string DefineName()
    {
        while (true)
        {
            string? title = System.Console.ReadLine();
            if (!string.IsNullOrEmpty(title) && title.All(char.IsLetter))
                return title;
            System.Console.WriteLine("invalid data format! Try again :)");
        }
    }

    private string DefineClientName()
    {
        System.Console.WriteLine("Insert client's name:");
        string name = DefineName();
        return name;
    }

    private string DefineClientSurname()
    {
        System.Console.WriteLine("Insert client's surname:");
        string name = DefineName();
        return name;
    }

    private string DefineBankTitle()
    {
        System.Console.WriteLine("Insert title of the bank:");
        string title = DefineName();
        return title;
    }

    private string? DefineAddress()
    {
        while (true)
        {
            System.Console.WriteLine("Insert address:");
            string? address = System.Console.ReadLine();
            string? addressCopy = address;
            if (addressCopy == null)
                return address;
            addressCopy = addressCopy.Replace(',', ' ');
            string[] data = addressCopy.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (data.Length != Client.AddressParametersCount ||
                (data[0].All(char.IsLetter) && data[1].All(char.IsLetter)) == false ||
                (data[2].All(char.IsDigit) && data[3].All(char.IsDigit) && data[4].All(char.IsDigit) == false))
            {
                System.Console.WriteLine("invalid data format! Try again :)");
                continue;
            }

            return address;
        }
    }

    private string? DefinePassportNumber()
    {
        while (true)
        {
            System.Console.WriteLine("Insert passport number:");
            string? passportNumber = System.Console.ReadLine();
            if (passportNumber == null || passportNumber.All(char.IsDigit))
                return passportNumber;
            System.Console.WriteLine("invalid data format! Try again :)");
        }
    }

    private bool DefineNotifications()
    {
        while (true)
        {
            System.Console.WriteLine("Insert true if you want to get notifications else false");
            if (bool.TryParse(System.Console.ReadLine(), out bool res))
                return res;
            System.Console.WriteLine("invalid data format! Try again :)");
        }
    }

    private int DefineBankId()
    {
        System.Console.WriteLine("Insert bank id(you can watch all ids and titles using command show_banks)");
        int id = DefineSum();
        return id;
    }

    private int DefineClientId()
    {
        System.Console.WriteLine("Insert client id(you can watch all information about clients using command show_clients_of_bank)");
        int id = DefineSum();
        return id;
    }

    private int DefineAccountId()
    {
        System.Console.WriteLine("Insert account id(you can watch all information about accounts using command show_client_accounts)");
        int id = DefineSum();
        return id;
    }

    private int TryFindBank()
    {
        Id:
        int id = DefineBankId();
        if (id < MinimalId)
        {
            System.Console.WriteLine($"Id must be >= {MinimalId}\n" +
                                     $"Try again :)");
            goto Id;
        }

        return id < _centralBank.Banks.Count ? id : BankErrorNumber;
    }

    private int TryFindAccount(int bankId, int clientId)
    {
        Id:
        int id = DefineAccountId();
        if (id < MinimalId)
        {
            System.Console.WriteLine($"Id must be >= {MinimalId}\n" +
                                     $"Try again :)");
            goto Id;
        }

        Client client = _centralBank.GetBankById(bankId).GetClient(clientId);
        List<Account> accounts = _centralBank.GetBankById(bankId).GetClientAccounts(client);
        return accounts.Find(account => account.Id == id) == null ? ClientErrorNumber : id;
    }

    private int TryFindClient(int bankId)
    {
        Id:
        int id = DefineClientId();
        if (id < MinimalId)
        {
            System.Console.WriteLine($"Id must be >= {MinimalId}\n" +
                                     $"Try again :)");
            goto Id;
        }

        return _centralBank.GetBankById(bankId).FindClient(id) == null ? AccountErrorNumber : id;
    }

    private Client? FindClient(out Bank? resBank)
    {
        int bankId = TryFindBank();
        if (bankId == -1)
        {
            System.Console.WriteLine("There is no bank with id you inserted");
            resBank = null;
            return null;
        }

        Bank bank = _centralBank.GetBankById(bankId);

        int clientId = TryFindClient(bankId);
        if (clientId == -1)
        {
            System.Console.WriteLine($"There is no client with id you inserted in bank {bank.Title}");
            resBank = bank;
            return null;
        }

        Client client = bank.GetClient(clientId);
        resBank = bank;
        return client;
    }

    private Bank? FindBank()
    {
        int bankId = TryFindBank();
        if (bankId == -1)
        {
            System.Console.WriteLine("There is no bank with id you inserted");
            return null;
        }

        Bank bank = _centralBank.GetBankById(bankId);
        return bank;
    }

    private Account? FindAccount(out Bank? resBank)
    {
        int bankId = TryFindBank();
        if (bankId == -1)
        {
            System.Console.WriteLine("There is no bank with id you inserted");
            resBank = null;
            return null;
        }

        Bank bank = _centralBank.GetBankById(bankId);

        int clientId = TryFindClient(bankId);
        if (clientId == -1)
        {
            System.Console.WriteLine($"There is no client with id you inserted in bank {bank.Title}");
            resBank = null;
            return null;
        }

        Client client = bank.GetClient(clientId);

        int accountId = TryFindAccount(bankId, clientId);
        Exit:
        if (accountId == -1)
        {
            System.Console.WriteLine($"Client with id {clientId} does not have account with id you inserted in bank {bank.Title}");
            resBank = null;
            return null;
        }

        List<Account> accounts = bank.GetClientAccounts(client);
        Account? account = accounts.Find(account => account.Id == accountId);
        if (account == null)
            goto Exit;
        resBank = bank;
        return account;
    }

    private bool CheckCommand(string? command) => command != null && _commands.ContainsKey(command);
}