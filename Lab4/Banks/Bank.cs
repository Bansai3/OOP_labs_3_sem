using System.Collections.ObjectModel;
namespace Banks;

public class Bank
{
    public const int InitialClientIdIdentifier = 1;
    public const int InitialAccountIdIdentifier = 1;
    private Dictionary<Client, List<Account>> _clients;
    private List<Account> _dubiousAccounts;
    private Dictionary<int, decimal> _depositPercents;
    private List<Client> _debitNotificationClients;
    private List<Client> _depositNotificationClients;
    private List<Client> _creditNotificationClients;
    private BankCharacteristicParameters _bankCharacteristicParameters;

    public Bank(
        Dictionary<int, decimal> depositPercents,
        decimal debitPercent,
        decimal creditCommissionPercent,
        int creditLimit,
        decimal transferCommission,
        int dubiousSum,
        int depositAccountMinDurationInDays,
        string title)
    {
        Validate(
            depositPercents,
            debitPercent,
            creditCommissionPercent,
            creditLimit,
            transferCommission,
            dubiousSum,
            depositAccountMinDurationInDays,
            title);
        _bankCharacteristicParameters = new BankCharacteristicParameters(
            debitPercent,
            creditCommissionPercent,
            transferCommission,
            depositAccountMinDurationInDays,
            creditLimit,
            dubiousSum);
        _depositPercents = new Dictionary<int, decimal>(depositPercents);
        _clients = new Dictionary<Client, List<Account>>();
        _dubiousAccounts = new List<Account>();
        _debitNotificationClients = new List<Client>();
        _depositNotificationClients = new List<Client>();
        _creditNotificationClients = new List<Client>();
        ClientIdIdentifier = InitialClientIdIdentifier;
        AccountIdIdentifier = InitialAccountIdIdentifier;
        Title = title;
    }

    public int ClientIdIdentifier { get; private set; }
    public int AccountIdIdentifier { get; private set; }
    public ReadOnlyCollection<Client> Clients => new (_clients.Keys.ToList());
    public string Title { get; private set; }

    public Client AddClient(string name, string surname, string? address = null, string? passportNumber = null, bool getCommonNotifications = false)
    {
        ClientBuilder clientBuilder = new ConcreteClientBuilder();
        Client newClient = clientBuilder
            .BuildName(name)
            .BuildSurname(surname)
            .BuildAddress(address)
            .BuildId(ClientIdIdentifier++)
            .BuildPassportNumber(passportNumber)
            .BuildCommonNotifications(getCommonNotifications)
            .BuildClient();
        _clients.Add(newClient, new List<Account>());
        return newClient;
    }

    public void AddClient(Client newClient)
    {
        if (CheckNewClient(newClient) == false)
            throw new ArgumentException("Invalid client!");
        _clients.Add(newClient, new List<Account>());
    }

    public DebitAccount OpenDebitAccount(Client client, decimal sum = 0, bool getNotification = false)
    {
        if (CheckClient(client) == false)
            throw new ArgumentException("Invalid client!");
        var debitAccount = new DebitAccount(_bankCharacteristicParameters.DebitPercent, AccountIdIdentifier, sum);
        _clients[client].Add(debitAccount);
        if (client.Address == null || client.PassportNumber == null)
            _dubiousAccounts.Add(debitAccount);
        if (getNotification)
            _debitNotificationClients.Add(client);
        return debitAccount;
    }

    public DepositAccount OpenDepositAccount(Client client, int durationInDays, decimal sum, bool getNotification = false)
    {
        if (CheckClient(client) == false)
            throw new ArgumentException("Invalid client!");
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid sum!");
        if (CheckDuration(durationInDays) == false)
            throw new ArgumentException("Invalid duration!");
        if (durationInDays < _bankCharacteristicParameters.DepositAccountMinDurationInDays)
            throw new InvalidOperationException("Can not open deposit account with duration less than minimal one!");
        decimal depositAccountPercent = DefinePercentForDepositAccount(sum);
        var newDepositAccount = new DepositAccount(depositAccountPercent, AccountIdIdentifier++, durationInDays, sum);
        _clients[client].Add(newDepositAccount);
        if (client.Address == null || client.PassportNumber == null)
            _dubiousAccounts.Add(newDepositAccount);
        if (getNotification)
            _depositNotificationClients.Add(client);
        return newDepositAccount;
    }

    public CreditAccount OpenCreditAccount(Client client, decimal sum, bool getNotification = false)
    {
        if (CheckClient(client) == false)
            throw new ArgumentException("Invalid client!");
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid sum!");
        if (sum > _bankCharacteristicParameters.CreditLimit)
            throw new ArithmeticException("Credit account sum is larger than credit limit!");
        var newCreditAccount = new CreditAccount(sum, _bankCharacteristicParameters.CreditCommissionPercent, AccountIdIdentifier++);
        _clients[client].Add(newCreditAccount);
        if (client.Address == null || client.PassportNumber == null)
            _dubiousAccounts.Add(newCreditAccount);
        if (getNotification)
            _creditNotificationClients.Add(client);
        return newCreditAccount;
    }

    public void TakeOffMoneyFromAccount(Account account, decimal sum)
    {
        if (CheckAccount(account) == false)
            throw new ArgumentException("Invalid account!");
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid sum!");
        if (_dubiousAccounts.Contains(account) && sum > _bankCharacteristicParameters.DubiousSum)
            throw new InvalidOperationException("Can not take off more money than dubious sum from dubious account!");
        if (CheckAccountExisting(account) == false)
            throw new ArgumentException("Account does not exist!");
        account.TakeOff(sum);
    }

    public void TakeOffMoneyFromAccount(int id, decimal sum)
    {
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid sum!");
        if (CheckId(id) == false)
            throw new ArgumentException("Invalid id!");
        Account? accountToFind = FindAccountById(id);
        if (accountToFind == null)
            throw new InvalidOperationException($"There is no account with id = {id}");
        if (_dubiousAccounts.Contains(accountToFind) && sum > _bankCharacteristicParameters.DubiousSum)
            throw new InvalidOperationException("Can not take off more money than dubious sum from dubious account!");
        accountToFind.TakeOff(sum);
    }

    public void TopUpMoneyOnAccount(Account account, decimal sum)
    {
        if (CheckAccount(account) == false)
            throw new ArgumentException("Invalid account!");
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid sum!");
        if (CheckAccountExisting(account) == false)
            throw new ArgumentException("Account does not exist!");
        account.TopUp(sum);
    }

    public void TopUpMoneyOnAccount(int id, decimal sum)
    {
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid sum!");
        if (CheckId(id) == false)
            throw new ArgumentException("Invalid Id!");
        Account? accountToFind = FindAccountById(id);
        if (accountToFind == null)
            throw new InvalidOperationException($"There is no account with id = {id}");
        accountToFind.TopUp(sum);
    }

    public void TransferMoneyToAnotherAccount(Bank bank, Account accountInThisBank, Account accountInAnotherBank, decimal sum)
    {
        if (CheckBank(bank) == false)
            throw new ArgumentException("Invalid bank!");
        if (CheckAccount(accountInThisBank) == false)
            throw new ArgumentException("Invalid accountInThisBank!");
        if (CheckAccount(accountInAnotherBank) == false)
            throw new ArgumentException("Invalid accountInAnotherBank!");
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid sum!");
        if (bank.CheckAccountExisting(accountInAnotherBank) == false)
            throw new InvalidOperationException("Can not transfer money to account that does not exist in bank!");
        if (CheckAccountExisting(accountInThisBank) == false)
            throw new InvalidOperationException("Can not transfer money from account that does not exist!");
        if (_dubiousAccounts.Contains(accountInThisBank) && sum > _bankCharacteristicParameters.DubiousSum)
            throw new InvalidOperationException("Can not transfer more money than dubious sum from dubious account!");
        decimal commission;
        if (bank != this)
            commission = sum * _bankCharacteristicParameters.TransferCommission;
        else
            commission = 0;
        accountInThisBank.Transfer(sum - commission, bank, accountInAnotherBank);
    }

    public Account? FindAccountById(int id)
    {
        foreach (KeyValuePair<Client, List<Account>> client in _clients)
        {
            Account? accountToFind = client.Value.FirstOrDefault(account => account.Id == id);
            if (accountToFind != null)
                return accountToFind;
        }

        return null;
    }

    public void CancelTransaction(Bank bank, Account accountInThisBank, Account accountInAnotherBank, decimal sum)
    {
        if (CheckBank(bank) == false)
            throw new ArgumentException("Invalid bank!");
        if (CheckAccount(accountInThisBank) == false)
            throw new ArgumentException("Invalid account!");
        if (CheckAccount(accountInAnotherBank) == false)
            throw new ArgumentException("Invalid account!");
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid sum!");
        if (bank.CheckAccountExisting(accountInAnotherBank) == false)
            throw new InvalidOperationException("Can not transfer money to account that does not exist in bank!");
        if (CheckAccountExisting(accountInThisBank) == false)
            throw new InvalidOperationException("Can not transfer money from account that does not exist!");
        accountInAnotherBank.TakeOff(sum);
        accountInThisBank.TopUp(sum);
    }

    public void ChangeClientName(Client client, string newName)
    {
        if (CheckClient(client) == false)
            throw new ArgumentException("Invalid client!");
        client.SetName(newName);
    }

    public void ChangeClientSurname(Client client, string newSurname)
    {
        if (CheckClient(client) == false)
            throw new ArgumentException("Invalid client!");
        client.SetName(newSurname);
    }

    public void ChangeClientAddress(Client client, string? address)
    {
        if (CheckClient(client) == false)
            throw new ArgumentException("Invalid client!");
        client.SetAddress(address);
        if (address != null && client.PassportNumber != null)
        {
            MakeClientNormal(client);
        }
    }

    public void ChangeClientPassportNumber(Client client, string? passportNumber)
    {
        if (CheckClient(client) == false)
            throw new ArgumentException("Invalid client!");
        client.SetAddress(passportNumber);
        if (passportNumber != null && client.Address != null)
        {
            MakeClientNormal(client);
        }
    }

    public void ChangeDebitPercent(decimal newDebitPercent)
    {
        if (CheckPercent(newDebitPercent) == false)
            throw new ArgumentException("Invalid debit percent!");
        decimal oldDebitPercent = _bankCharacteristicParameters.DebitPercent;
        _bankCharacteristicParameters.SetDebitPercent(newDebitPercent);
        foreach (Client client in _debitNotificationClients)
        {
            client.Notify(new BankApplicationNotification($"Debit percent changed from {oldDebitPercent}% to {newDebitPercent}%!"));
        }
    }

    public void ChangeDepositPercents(Dictionary<int, decimal> depositPercents)
    {
        if (CheckDepositPercents(depositPercents) == false)
            throw new ArgumentException("Invalid deposit percents!");
        _depositPercents = new Dictionary<int, decimal>(depositPercents);
        foreach (Client client in _depositNotificationClients)
        {
            client.Notify(new BankApplicationNotification("Deposit percents changed!"));
        }
    }

    public void ChangeCreditCommissionPercent(decimal creditCommissionPercent)
    {
        if (CheckPercent(creditCommissionPercent) == false)
            throw new ArgumentException("Invalid credit commission percent!");
        decimal oldCreditCommissionPercent = _bankCharacteristicParameters.CreditCommissionPercent;
        _bankCharacteristicParameters.SetCreditCommissionPercent(creditCommissionPercent);
        foreach (Client client in _creditNotificationClients)
        {
            client.Notify(new BankApplicationNotification($"Credit commission percent changed from {oldCreditCommissionPercent} to {creditCommissionPercent}!"));
        }
    }

    public void ChangeTransferCommission(decimal transferCommission)
    {
        if (CheckPercent(transferCommission) == false)
            throw new ArgumentException("Invalid transfer commission!");
        decimal oldTransferCommission = _bankCharacteristicParameters.TransferCommission;
        _bankCharacteristicParameters.SetTransferCommission(transferCommission);
        foreach (KeyValuePair<Client, List<Account>> client in _clients)
        {
            if (client.Key.GetCommonNotifications)
                client.Key.Notify(new BankApplicationNotification($"Transfer commission changed from {oldTransferCommission}% to {transferCommission}%!"));
        }
    }

    public void ChangeDepositAccountMinDurationInDays(int depositAccountMinDurationInDays)
    {
        if (CheckDuration(depositAccountMinDurationInDays) == false)
            throw new ArgumentException("Invalid deposit account minimal duration in days!");
        int oldDepositAccountMinDurationInDays = _bankCharacteristicParameters.DepositAccountMinDurationInDays;
        _bankCharacteristicParameters.SetDepositAccountMinDurationInDays(depositAccountMinDurationInDays);
        foreach (Client client in _depositNotificationClients)
        {
            client.Notify(new BankApplicationNotification($"Changed deposit account minimal duration from {oldDepositAccountMinDurationInDays} days to {depositAccountMinDurationInDays} days!"));
        }
    }

    public void ChangeCreditLimit(int creditLimit)
    {
        if (CheckCreditLimit(creditLimit) == false)
            throw new ArgumentException("Invalid credit limit!");
        int oldCreditLimit = _bankCharacteristicParameters.CreditLimit;
        _bankCharacteristicParameters.SetCreditLimit(creditLimit);
        foreach (Client client in _creditNotificationClients)
        {
            client.Notify(new BankApplicationNotification($"Credit limit changed from {oldCreditLimit} rubles to {creditLimit} rubles!"));
        }
    }

    public void ChangeDubiousSum(int dubiousSum)
    {
        if (CheckDubiousSum(dubiousSum) == false)
            throw new ArgumentException("Invalid dubious sum!");
        int olDubiousSum = _bankCharacteristicParameters.DubiousSum;
        _bankCharacteristicParameters.SetDubiousSum(dubiousSum);
        foreach (KeyValuePair<Client, List<Account>> client in _clients)
        {
            if (client.Key.GetCommonNotifications)
                client.Key.Notify(new BankApplicationNotification($"Dubious sum changed from {olDubiousSum} rubles to {dubiousSum} rubles!"));
        }
    }

    public Client GetClient(int id)
    {
        if (CheckId(id) == false)
            throw new ArgumentException("Invalid id!");
        Client? client = _clients.Keys.FirstOrDefault(client => client.Id == id);
        if (client == null)
            throw new ArgumentException($"There is no client with id: {id}");
        return client;
    }

    public Client? FindClient(int id) => _clients.Keys.FirstOrDefault(client => client.Id == id);

    public void UpdatePercentsAndCommission(bool endOfMonth)
    {
        foreach (KeyValuePair<Client, List<Account>> client in _clients)
        {
            foreach (Account account in client.Value)
            {
                account.AccruePercent();
                if (endOfMonth)
                    account.AccruePercentSum();
            }
        }
    }

    public List<Account> GetClientAccounts(Client client)
    {
        if (CheckClient(client) == false)
            throw new ArgumentException("Invalid client!");
        return _clients[client];
    }

    private void MakeClientNormal(Client client)
    {
        if (CheckClient(client) == false)
            throw new ArgumentException("Invalid client!");
        foreach (Account account in _clients[client])
        {
            _dubiousAccounts.Remove(account);
        }
    }

    private bool CheckAccount(Account? account) => account != null;
    private decimal DefinePercentForDepositAccount(decimal sum)
    {
        decimal maxPercent = 0;
        foreach (KeyValuePair<int, decimal> sumPercent in _depositPercents)
        {
            if (sum <= sumPercent.Key)
                return sumPercent.Value;
            maxPercent = sumPercent.Value;
        }

        return maxPercent;
    }

    private bool CheckPercent(decimal percent) => percent > 0;
    private bool CheckCreditLimit(int creditLimit) => creditLimit > 0;
    private bool CheckDepositPercents(Dictionary<int, decimal>? depositPercents)
    {
        if (depositPercents == null)
            return false;
        int currentKey = -1;
        decimal currentDepositPercent = -1;
        foreach (KeyValuePair<int, decimal> sumPercent in depositPercents)
        {
            if (sumPercent.Key < 0 || sumPercent.Value < 0)
                return false;
            if (currentKey < sumPercent.Key && currentDepositPercent < sumPercent.Value)
            {
                currentKey = sumPercent.Key;
                currentDepositPercent = sumPercent.Value;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckDuration(int durationInDays) => durationInDays > 0;
    private bool CheckSum(decimal sum) => sum >= 0;
    private bool CheckAccountExisting(Account account) => _clients.Any(client => client.Value.Contains(account));
    private bool CheckDubiousSum(int dubiousSum) => dubiousSum > 0;
    private bool CheckClient(Client? client) => client != null && _clients.ContainsKey(client);
    private bool CheckId(int id) => _clients.Any(client => client.Key.Id == id);
    private bool CheckBank(Bank? bank) => bank != null;
    private bool CheckNewClient(Client? client) => client != null && _clients.ContainsKey(client) == false;
    private bool CheckTitle(string title) => !string.IsNullOrEmpty(title) && title.All(char.IsLetter);

    private void Validate(
        Dictionary<int, decimal> depositPercents,
        decimal debitPercent,
        decimal creditCommissionPercent,
        int creditLimit,
        decimal transferCommission,
        int dubiousSum,
        int depositAccountMinDurationInDays,
        string title)
    {
        if (CheckDepositPercents(depositPercents) == false)
            throw new ArgumentException("Invalid depositPercents!");
        if (CheckPercent(debitPercent) == false)
            throw new ArgumentException("Invalid debit percent!");
        if (CheckPercent(creditCommissionPercent) == false)
            throw new ArgumentException("Invalid commission percent!");
        if (CheckCreditLimit(creditLimit) == false)
            throw new ArgumentException("Invalid credit limit!");
        if (CheckPercent(transferCommission) == false)
            throw new ArgumentException("Invalid transfer commission!");
        if (CheckDubiousSum(dubiousSum) == false)
            throw new ArgumentException("Invalid dubious sum!");
        if (CheckDuration(depositAccountMinDurationInDays) == false)
            throw new ArgumentException("Invalid deposit account minimal duration!");
        if (CheckTitle(title) == false)
            throw new ArgumentException("Invalid title!");
    }
}