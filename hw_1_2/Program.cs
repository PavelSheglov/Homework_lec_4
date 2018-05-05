using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw_1_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var client1 = new IndividualBusinessman();
            var client2 = new LimitedOrganization();

            client1.SetMainParameters("111111111111", "(111) 1111111", 11111.11M);
            client1.SetFullName("Иванов", "Иван", "Иванович");
            client1.SetBirthDate(20,5,1985);

            client2.SetMainParameters("2222222222", "(2222) 222222", 222222222.22M);
            client2.SetName("Самая лучшая компания");
            client2.SetAccount("40702999200000000222");

            Console.WriteLine(client1.GetInformation());
            Console.WriteLine();
            Console.WriteLine(client2.GetInformation());
        }
    }

    public abstract class Client
    {
        private string _id;
        private string _phoneNumber;
        private decimal _orderAmount;

        protected string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        protected string MainPhone
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }

        protected decimal OrderAmount
        {
            get { return _orderAmount; }
            set { _orderAmount = value; }
        }

        public void SetMainParameters(string id, string phone, decimal amount)
        {
            Id = id;
            MainPhone = phone;
            OrderAmount = amount;
        }

        public abstract string GetInformation();
    }

    public sealed class IndividualBusinessman:Client
    {
        private string _surname;
        private string _firstname;
        private string _patronymic;
        private byte _birthDay;
        private byte _birthMonth;
        private ushort _birthYear;

        public IndividualBusinessman()
        {
            Id = "000000000000";
            MainPhone = "0000000";
            OrderAmount = 0.00M;
            _surname = "";
            _firstname = "";
            _patronymic = "";
            _birthDay = 0;
            _birthMonth = 0;
            _birthYear = 0;
        }

        public IndividualBusinessman(string id, string phone, decimal amount, string surname, string firstname, string patronymic, byte birthday, byte birthmonth, ushort birthyear)
        {
            SetMainParameters(id, phone, amount);
            SetFullName(surname, firstname, patronymic);
            SetBirthDate(birthday, birthmonth, birthyear);
        }

        public IndividualBusinessman(string id, string phone, decimal amount, string surname, string firstname, byte birthday, byte birthmonth, ushort birthyear):this(id, phone,amount,surname,firstname,"",birthday, birthmonth,birthyear)
        {

        }
        
        public void SetFullName(string surname, string firstname, string patronymic)
        {
            _surname = surname;
            _firstname = firstname;
            _patronymic = patronymic;
        }       

        public void SetBirthDate(byte birthday, byte birthmonth, ushort birthyear)
        {
            _birthDay = birthday;
            _birthMonth = birthmonth;
            _birthYear = birthyear;
        }

        private string FullName => string.Format("{0} {1} {2}", _surname, _firstname, _patronymic);

        private string BirthDate => string.Format("{0:d2}.{1:d2}.{2:d4} г.", _birthDay, _birthMonth, _birthYear);

        public override string GetInformation()
        {
            var information=new StringBuilder(1000);
            information.AppendFormat("Информация по клиенту:\n");
            information.AppendFormat("--------------------------------------------------------------------\n");
            information.AppendFormat("Индивидуальный предприниматель {0}, ID {1},\n", FullName, Id);
            information.AppendFormat("дата рождения:{0}, телефон:{1}\n", BirthDate, MainPhone);
            information.AppendFormat("--------------------------------------------------------------------\n");
            information.AppendFormat("Сумма заказа:{0:0,#.00} денежных единиц", OrderAmount);
            return information.ToString();
        }
    }

    public sealed class LimitedOrganization:Client
    {
        private string _name;
        private string _account;

        public LimitedOrganization()
        {
            Id = "0000000000";
            MainPhone = "0000000";
            OrderAmount = 0.00M;
        }

        public LimitedOrganization(string id, string phone, decimal amount, string name, string account)
        {
            SetMainParameters(id, phone, amount);
            SetName(name);
            SetAccount(account);
        }

        public void SetName(string name)
        {
            _name = name;
        }

        public void SetAccount(string account)
        {
            _account = account;
        }

        public override string GetInformation()
        {
            var information = new StringBuilder(1000);
            information.AppendFormat("Информация по клиенту:\n");
            information.AppendFormat("--------------------------------------------------------------------\n");
            information.AppendFormat("Общество с ограниченной ответственностью\n{0},\nID {1},\n", _name, Id);
            information.AppendFormat("расчетный счет:{0}, телефон:{1}\n", _account, MainPhone);
            information.AppendFormat("--------------------------------------------------------------------\n");
            information.AppendFormat("Сумма заказа:{0:0,#.00} денежных единиц", OrderAmount);
            return information.ToString();
        }
    }
}
