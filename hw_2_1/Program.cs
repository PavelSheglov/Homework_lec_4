using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw_2_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var client1 = new Client("Иванов Иван Иванович");
            var client2 = new Client("Петров Петр Петрович ");
            var rnd = new Random();

            client1.OpenSavingsAccount((decimal)rnd.Next(100, 3000));
            client1.OpenCumulativeAccount((decimal)rnd.Next(10000, 15000));
            client1.OpenCheckingAccount((decimal)rnd.Next(100, 5000));
            client1.OpenMetalAccount(MetalType.Argentum, (uint)rnd.Next(100, 300), 1500.00M);
            client1.OpenMetalAccount(MetalType.Aurum, (uint)rnd.Next(100, 300), 4500.00M);

            client2.OpenSavingsAccount((decimal)rnd.Next(100, 3000));
            client2.OpenCumulativeAccount((decimal)rnd.Next(10000, 15000));
            client2.OpenCheckingAccount((decimal)rnd.Next(100, 5000));
            client2.OpenMetalAccount(MetalType.Platinum, (uint)rnd.Next(100, 300), 6500.00M);
            client2.OpenMetalAccount(MetalType.Aurum, (uint)rnd.Next(100, 300), 4500.00M);
            client2.OpenMetalAccount(MetalType.Argentum, (uint)rnd.Next(100, 300), 1500.00M);

            Console.WriteLine("-----------Демонстрация работы с клиентами------------");
            Console.WriteLine("Нажмите клавишу.....");
            Console.ReadKey();
            Console.WriteLine(client1.ToString());
            Console.WriteLine();
            Console.WriteLine(client2.ToString());
            Console.WriteLine();
            switch(client1.CompareTo(client2))
            {
                case 1:
                    Console.WriteLine("Клиент {0} больше клиента {1}", client1.Name, client2.Name);
                    break;
                case -1:
                    Console.WriteLine("Клиент {0} меньше клиента {1}", client1.Name, client2.Name);
                    break;
                case 0:
                    Console.WriteLine("Клиент {0} равен клиенту {1}", client1.Name, client2.Name);
                    break;
            }
                       
            Console.WriteLine();
            Console.WriteLine("-----------Демонстрация работы со счетами------------");

            Console.WriteLine("Нажмите клавишу.....");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("-----------Сберегательный счет------------");
            var account1 = new SavingsAccount("11111111", "client1");
            Console.WriteLine(account1.ToString());
            account1.AddFunds(2000.00M);
            Console.WriteLine(account1.ToString());
            account1.WithdrawFunds(500.50M);
            Console.WriteLine(account1.ToString());
            account1.ZeroingAccount();
            Console.WriteLine(account1.ToString());
            account1.CloseAccount();
            Console.WriteLine(account1.ToString());
            account1.AddFunds(500);

            Console.WriteLine("Нажмите клавишу.....");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("-----------Накопительный счет------------");
            var account2 = new CumulativeAccount("22222222", "client2", 20000);
            Console.WriteLine(account2.ToString());
            account2.AddFunds(1000.00M);
            Console.WriteLine(account2.ToString());
            account2.WithdrawFunds(1000.00M);
            Console.WriteLine(account2.ToString());
            account2.WithdrawFunds(1000.00M);
            Console.WriteLine(account2.ToString());
            account2.InterestsCapitalization();
            Console.WriteLine(account2.ToString());
            account2.ZeroingAccount();
            Console.WriteLine(account2.ToString());
            account2.CloseAccount();
            Console.WriteLine(account2.ToString());

            Console.WriteLine("Нажмите клавишу.....");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("-----------Расчетный счет------------");
            var account3 = new CheckingAccount("33333333", "client3");
            Console.WriteLine(account3.ToString());
            account3.AddFunds(1000.00M);
            Console.WriteLine(account3.ToString());
            account3.WithdrawFunds(500.00M);
            Console.WriteLine(account3.ToString());
            account3.WithdrawFunds(500.00M);
            Console.WriteLine(account3.ToString());
            account3.ZeroingAccount();
            Console.WriteLine(account3.ToString());
            account3.CloseAccount();
            Console.WriteLine(account3.ToString());

            Console.WriteLine("Нажмите клавишу.....");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("-----------Металлический счет------------");
            var account4 = new MetalAccount("444444444", "client4", MetalType.Aurum, 200, 1500.50M);
            Console.WriteLine(account4.ToString());
            account4.AddFunds(500);
            Console.WriteLine(account4.ToString());
            account4.WithdrawFunds(300);
            Console.WriteLine(account4.ToString());
            account4.ZeroingAccount();
            Console.WriteLine(account4.ToString());
            account4.CloseAccount();
            Console.WriteLine(account4.ToString());
        }
    }

    public enum AccountStatus
    {
        Opened,
        Closed
    }

    public enum MetalType
    {
        Aurum=100,
        Argentum,
        Platinum
    }

    public abstract class BankAccount
    {
        private string _number;
        private string _owner;
        private decimal _balance;
        private AccountStatus _status;

        protected BankAccount()
        {
            _number = "";
            _owner = "";
            _balance = 0.00M;
            _status = AccountStatus.Opened;
        }

        protected BankAccount(string number, string owner):this()
        {
            _number = number;
            _owner = owner;
        }

        public string AccountNumber
        {
            get { return _number; }
            set { if(_status==AccountStatus.Opened && _number=="") _number = value; }
        }

        public string AccountOwner
        {
            get { return _owner; }
            set { if (_status == AccountStatus.Opened && _owner=="") _owner = value; }
        }

        public decimal AccountBalance => _balance;

        public string Status => _status.ToString();

        protected bool IncreaseBalance(decimal delta)
        {
            if (_status == AccountStatus.Opened && delta >= 0)
            {
                _balance += Math.Truncate(delta*100M)/100M;
                return true;
            }
            else
                return false;
        }

        protected bool DecreaseBalance(decimal delta)
        {
            if (_status == AccountStatus.Opened && delta >= 0 && (_balance-delta)>=0)
            {
                _balance -= Math.Truncate(delta * 100M) / 100M;
                return true;
            }
            else
                return false;
        }

        protected bool Close()
        {
            if (_status == AccountStatus.Opened && _balance == 0)
            {
                _status = AccountStatus.Closed;
                return true;
            }
            else
                return false;
        }      
    }

    public sealed class SavingsAccount : BankAccount
    {
        private SavingsAccount():base() {}
        public SavingsAccount(string clientAccount, string clientName) :base(clientAccount, clientName) {}

        public void AddFunds(decimal amount)
        {
            if (this.IncreaseBalance(amount))
                Console.WriteLine("Счет успешно пополнен на {0:0.00} д.е.", amount);
            else
                Console.WriteLine("Счет закрыт или указана неверная сумма пополнения");
        }

        public void WithdrawFunds(decimal amount)
        {
            if (this.DecreaseBalance(amount))
                Console.WriteLine("Со счета успешно списано {0:0.00} д.е.", amount);
            else
                Console.WriteLine("Счет закрыт, недостаточно средств на счете или указана неверная сумма списания");
        }

        public void ZeroingAccount()
        {
            if (this.DecreaseBalance(AccountBalance))
                Console.WriteLine("Счет успешно обнулен!");
            else
                Console.WriteLine("Счет не может быть обнулен, т.к. закрыт!");
        }

        public void CloseAccount()
        {
            if (this.Close())
                Console.WriteLine("Счет успешно закрыт!");
            else
                Console.WriteLine("Счет не может быть закрыт, т.к. он уже закрыт или на нем есть средства!");

        }

        public override string ToString()
        {
            var str = string.Format("Клиент:{0}\n, сберегательный счет:{1}, остаток:{2:0.00} д.е.\n",this.AccountOwner, this.AccountNumber, this.AccountBalance);
            str += string.Format("статус:{0}", this.Status);
            return str;
        }
    }

    public sealed class CumulativeAccount : BankAccount
    {
        private decimal _initialBalance=10000.00M;
        private decimal _rate = 2.5M; //%

        private CumulativeAccount() :base() { }

        public CumulativeAccount(string clientAccount, string clientName) : this(clientAccount, clientName, 0) { }

        public CumulativeAccount(string clientAccount, string clientName, decimal initAmount) : base(clientAccount, clientName)
        {
            if (initAmount > 0) _initialBalance = initAmount;
            this.IncreaseBalance(_initialBalance);
            if (initAmount > 10000.00M && initAmount <= 100000.00M)
                _rate = 3.5M;
            else if (initAmount > 100000.00M)
                _rate = 4.5M;
        }

        public void AddFunds(decimal amount)
        {
            if (this.IncreaseBalance(amount))
                Console.WriteLine("Счет успешно пополнен на {0:0.00} д.е.", amount);
            else
                Console.WriteLine("Счет закрыт или указана неверная сумма пополнения");
        }

        public void WithdrawFunds(decimal amount)
        {
            if((this.AccountBalance - amount)>=_initialBalance)
                if (this.DecreaseBalance(amount))
                    Console.WriteLine("Со счета успешно списано {0:0.00} д.е.", amount);
                else
                Console.WriteLine("Счет закрыт, недостаточно средств на счете или указана неверная сумма списания");
            else
                Console.WriteLine("На счете не может быть сумма меньше неснижаемого остатка {0:0.00} д.е.!", _initialBalance);
        }

        public void ZeroingAccount()
        {
            if (this.DecreaseBalance(this.AccountBalance))
                Console.WriteLine("Счет успешно обнулен!");
            else
                Console.WriteLine("Счет не может быть обнулен, т.к.закрыт!");
        }

        public void CloseAccount()
        {
            if (this.Close())
                Console.WriteLine("Счет успешно закрыт!");
            else
                Console.WriteLine("Счет не может быть закрыт, т.к. он уже закрыт или на нем есть средства!");

        }

        public void InterestsCapitalization()
        {
            if (this.AccountBalance > 0)
                if(this.IncreaseBalance(this.AccountBalance/36000*_rate*30))
                    Console.WriteLine("Капитализация процентов успешно поведена");
                else
                    Console.WriteLine("Капитализация процентов не возможна в связи с закрытием счета");
            else
                Console.WriteLine("Капитализация процентов не возможна по причине отсутствия средств на счете");
        }

        public override string ToString()
        {
            var str = string.Format("Клиент:{0}\n, накопительный счет:{1}, остаток:{2:0.00} д.е.,\n", this.AccountOwner, this.AccountNumber, this.AccountBalance);
            str += string.Format("неснижаемый остаток:{0:0.00} д.е., процентная ставка:{1:0.00}%,\n", _initialBalance, _rate);
            str += string.Format("статус:{0}", this.Status);
            return str;
        }
    }

    public sealed class CheckingAccount : BankAccount
    {
        private decimal _fee = 100.00M;

        public CheckingAccount() : base() { }

        public CheckingAccount(string clientAccount, string clientName):base(clientAccount, clientName) { }

        public void AddFunds(decimal amount)
        {
            if (this.IncreaseBalance(amount))
                Console.WriteLine("Счет успешно пополнен на {0:0.00} денежных единиц", amount);
            else
                Console.WriteLine("Счет закрыт или указана неверная сумма пополнения");
        }

        public void WithdrawFunds(decimal amount)
        {
            if ((this.AccountBalance - amount) >= _fee)
            {
                this.WithdrawFee();
                if (this.DecreaseBalance(amount))
                    Console.WriteLine("Со счета успешно списано {0:0.00} д.е.", amount);
                else
                    Console.WriteLine("Счет закрыт, недостаточно средств на счете или указана неверная сумма списания");
            }
            else
                Console.WriteLine("На счете не может быть сумма меньше комиссии за обслуживание {0:0.00} д.е.!", _fee);
        }

        public void ZeroingAccount()
        {
            if (this.DecreaseBalance(AccountBalance))
                 Console.WriteLine("Счет успешно обнулен!");
            else
                 Console.WriteLine("Счет не может быть обнулен, т.к. закрыт!");
        }

        public void CloseAccount()
        {
            if (this.Close())
                Console.WriteLine("Счет успешно закрыт!");
            else
                Console.WriteLine("Счет не может быть закрыт, т.к. он уже закрыт или на нем есть средства!");

        }

        private void WithdrawFee()
        {
            if(this.DecreaseBalance(_fee))
                Console.WriteLine("Комиссия за обслуживание {0:0.00} д.е. списана", _fee);
            else
                Console.WriteLine("Комиссия не может быть списана, т.к. счет закрыт или недостаточно средств на счете!");
        }

        public override string ToString()
        {
            var str = string.Format("Клиент:{0}\n, расчетный счет:{1}, остаток:{2:0.00} д.е.,\n", this.AccountOwner, this.AccountNumber, this.AccountBalance);
            str += string.Format("стоимость платежа:{0:0.00} д.е.,\n", _fee);
            str += string.Format("статус:{0}", this.Status);
            return str;
        }
    }

    public sealed class MetalAccount : BankAccount
    {
        private MetalType _type;
        private uint _metalBalance;
        private decimal _priceRate;

        private MetalAccount() : base() { }

        public MetalAccount(string clientAccount, string clientName, MetalType metal, uint balance, decimal price):base(clientAccount, clientName)
        {
            _type = metal;
            _metalBalance = balance;
            _priceRate = price;
            this.IncreaseBalance((decimal)(_metalBalance * _priceRate));
        }

        public void AddFunds(uint weight)
        {
            if (this.IncreaseBalance((decimal)(weight * _priceRate)))
            {
                _metalBalance += weight;
                Console.WriteLine("Счет успешно пополнен на {0} г. ({1:0.00} д.е.)", weight, (decimal)(weight * _priceRate));
            }
            else
                Console.WriteLine("Счет закрыт или указана неверная сумма пополнения");
        }

        public void WithdrawFunds(uint weight)
        {
            if (this.DecreaseBalance((decimal)(weight * _priceRate)))
            {
                _metalBalance -= weight;
                Console.WriteLine("Со счета успешно списано {0} г. ({1:0.00} д.е.)", weight, (decimal)(weight * _priceRate));
            }
            else
                Console.WriteLine("Счет закрыт, недостаточно средств на счете или указана неверная сумма списания");
        }

        public void ZeroingAccount()
        {
            if (this.DecreaseBalance(AccountBalance))
            {
                _metalBalance = 0;
                Console.WriteLine("Счет успешно обнулен!");
            }
            else
                Console.WriteLine("Счет не может быть обнулен, т.к. закрыт!");
        }

        public void CloseAccount()
        {
            if (this.Close())
                Console.WriteLine("Счет успешно закрыт!");
            else
                Console.WriteLine("Счет не может быть закрыт, т.к. он уже закрыт или на нем есть средства!");

        }

        public override string ToString()
        {
            var str = string.Format("Клиент:{0}\n, металлический счет:{1}, остаток:{2:0.00} д.е.,\n", this.AccountOwner, this.AccountNumber, this.AccountBalance);
            str += string.Format("металл:{0}, остаток:{1:0} г., курс:{2:0.00} д.е./г.\n", _type.ToString(), _metalBalance, _priceRate);
            str += string.Format("статус:{0}", this.Status);
            return str;
        }
    }

    public sealed class Client : IComparable
    {
        private string _name;
        private uint _number;
        private List<SavingsAccount> SavingsAccounts;
        private List<CumulativeAccount> CumulativeAccounts;
        private List<CheckingAccount> CheckingAccounts;
        private List<MetalAccount> MetalAccounts;
        private uint _openedAccountsCount;
        private enum AccountType
        {
            Saving=100,
            Cumulative,
            Checking,
            Metal
        }


        private Client() { }

        public Client(string name)
        {
            _name = name;
            _number = (uint)(name.GetHashCode() + DateTime.Now.Second);
            SavingsAccounts = new List<SavingsAccount>();
            CumulativeAccounts = new List<CumulativeAccount>();
            CheckingAccounts = new List<CheckingAccount>();
            MetalAccounts = new List<MetalAccount>();
            _openedAccountsCount = 0;
        }

        public string Name => _name;

        public void OpenSavingsAccount(decimal initAmount)
        {
            var account = "ACC" + AccountType.Saving.ToString() + (_openedAccountsCount + 1).ToString() + "000" + _number.ToString();
            var newAccount = new SavingsAccount(account, _name);
            newAccount.AddFunds(initAmount);
            SavingsAccounts.Add(newAccount);
            _openedAccountsCount++;
        }

        public void OpenCumulativeAccount(decimal initAmount)
        {
            var account = "ACC" + AccountType.Cumulative.ToString() + (_openedAccountsCount + 1).ToString() + "000" + _number.ToString();
            var newAccount = new CumulativeAccount(account, _name, initAmount);
            CumulativeAccounts.Add(newAccount);
            _openedAccountsCount++;
        }

        public void OpenCheckingAccount(decimal initAmount)
        {
            var account = "ACC" + AccountType.Checking.ToString() + (_openedAccountsCount + 1).ToString() + "000" + _number.ToString();
            var newAccount = new CheckingAccount(account, _name);
            newAccount.AddFunds(initAmount);
            CheckingAccounts.Add(newAccount);
            _openedAccountsCount++;
        }

        public void OpenMetalAccount(MetalType type, uint initAmount, decimal price)
        {
            var account = "ACC" + AccountType.Metal.ToString() + (_openedAccountsCount + 1).ToString() + type.ToString() + _number.ToString();
            var newAccount = new MetalAccount(account, _name, type, initAmount, price);
            MetalAccounts.Add(newAccount);
            _openedAccountsCount++;
        }

        public void ZeroingAndCloseAllAccounts()
        {
            if(SavingsAccounts.Count>0)
            {
                foreach(var acc in SavingsAccounts)
                {
                    acc.ZeroingAccount();
                    acc.CloseAccount();
                    _openedAccountsCount--;
                }
            }
            if (CumulativeAccounts.Count > 0)
            {
                foreach (var acc in CumulativeAccounts)
                {
                    acc.ZeroingAccount();
                    acc.CloseAccount();
                    _openedAccountsCount--;
                }
            }
            if (CheckingAccounts.Count > 0)
            {
                foreach (var acc in CheckingAccounts)
                {
                    acc.ZeroingAccount();
                    acc.CloseAccount();
                    _openedAccountsCount--;
                }
            }
            if (MetalAccounts.Count > 0)
            {
                foreach (var acc in MetalAccounts)
                {
                    acc.ZeroingAccount();
                    acc.CloseAccount();
                    _openedAccountsCount--;
                }
            }
        }

        private decimal CalculateTotalBalance()
        {
            decimal total = 0;
            if(_openedAccountsCount>0)
            {
                if (SavingsAccounts.Count > 0)
                {
                    foreach (var acc in SavingsAccounts)
                        total += acc.AccountBalance;
                }
                if (CumulativeAccounts.Count > 0)
                {
                    foreach (var acc in CumulativeAccounts)
                        total += acc.AccountBalance;
                }
                if (CheckingAccounts.Count > 0)
                {
                    foreach (var acc in CheckingAccounts)
                        total += acc.AccountBalance;
                }
                if (MetalAccounts.Count > 0)
                {
                    foreach (var acc in MetalAccounts)
                        total += acc.AccountBalance;
                }
            }
            return total;
        }

        public decimal TotalBalance => CalculateTotalBalance();

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            var temp = obj as Client;
            if (this.TotalBalance > temp.TotalBalance)
                return 1;
            else if (this.TotalBalance < temp.TotalBalance)
                return -1;
            else
                return 0;
        }

        public override string ToString()
        {
            return string.Format("Клиент:{0},\nоткрытых счетов {1}, суммарный остаток на всех счетах {2:#.00}", _name, _openedAccountsCount, TotalBalance);
        }
    }
}
