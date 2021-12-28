using System;

namespace CommandPattern
{
    //интерфейс Команды объявляет метод для выполнения команд.
    public interface ICommand
    {
        void Execute();
    }

    //некоторые команды способны выполнять простые операции самостоятельно.
    class OnConditioner : ICommand //включение кондиционера
    {
        private string _payload = string.Empty; //заполнение пустотой

        public OnConditioner(string payload) //конструктор... по всей видимости для "логики" программы
        {
            this._payload = payload;
        }

        public void Execute() //метод... метод выполнения команды-класса
        {
            Console.WriteLine($"OnConditioner: Вывод в консоль - простое действие, можно оставить в команде ({this._payload})"); //ничего сложного => ничего страшного
        }
    }

    class OffConditioner : ICommand //выключение кондиционера
    {
        private string _payload = string.Empty; //заполнение пустотой

        public OffConditioner(string payload) //конструктор... по всей видимости для "логики" программы
        {
            this._payload = payload;
        }

        public void Execute() //метод... метод выполнения команды-класса
        {
            Console.WriteLine($"OnConditioner: команда аналогичная включению кондиционера. ({this._payload})"); //ничего сложного => ничего страшного
        }
    }

    //но есть и команды, которые делегируют более сложные операции другим
    //объектам, называемым «получателями».
    class OnKettle : ICommand //включение чайника
    {
        private Receiver _receiver;

        //данные о контексте, необходимые для запуска методов получателя.
        private string _a;

        private string _b;

        //сложные команды могут принимать один или несколько объектов-
        //получателей вместе с любыми данными о контексте через конструктор.
        public OnKettle(Receiver receiver, string a, string b)
        {
            this._receiver = receiver; //без этого работать не будет
            this._a = a;
            this._b = b;
        }

        //команды могут делегировать выполнение любым методам получателя.
        public void Execute()
        {
            Console.WriteLine("OnKettle: переход из команды в получателя");
            this._receiver.DoSomething(this._a);
            this._receiver.DoSomethingElse(this._b);
        }
    }

    class Receiver //получатель
    {
        public void DoSomething(string a)
        {
            Console.WriteLine($"Receiver: кнопка нажата ({a}.)");
        }

        public void DoSomethingElse(string b)
        {
            Console.WriteLine($"Receiver: воды достаточно ({b}.)");
        }
    }

    //отправитель связан с одной или несколькими командами. Он отправляет запрос команде 
    class Invoker //отправитель
    {
        private ICommand anConditioner;

        private ICommand anKettle;

        //инициализация команд
        public void ConditionerOn(ICommand command)
        {
            this.anConditioner = command;
        }
        public void ConditionerOff(ICommand command)
        {
            this.anConditioner = command;
        }

        public void KettleOn(ICommand command)
        {
            this.anKettle = command;
        }

        //отправитель не зависит от классов конкретных команд и получателей.
        //отправитель передаёт запрос получателю косвенно, выполняя команду.
        public void DoSomethingImportant()
        {
            Console.WriteLine("Invoker: имитация какой-нибудь работы до включения кондиционера"); 
            if (this.anConditioner is ICommand) //является ли this.anConditioner объектом типа ICommand (тривиальная проверка)
            {
                this.anConditioner.Execute(); //(в данном коде) выполнение простой команды
            }

            Console.WriteLine("Invoker: имитация какой-нибудь работы до выключения кондиционера");
            if (this.anConditioner is ICommand) //является ли this.anConditioner объектом типа ICommand
            {
                this.anConditioner.Execute(); //(в данном коде) выполнение простой команды
            }

            Console.WriteLine("Invoker: имитация какой-нибудь работы до включения чайника");
            if (this.anKettle is ICommand)
            {
                this.anKettle.Execute(); //(в данном коде) выполнение комплексной команды
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Invoker invoker = new Invoker();
            invoker.ConditionerOn(new OnConditioner("Кондиционер включен"));
            invoker.ConditionerOff(new OnConditioner("Кондиционер выключен"));
            Receiver receiver = new Receiver();
            invoker.KettleOn(new OnKettle(receiver, "Чайник включен", "Чайник работает")); //благодаря тому, что тут мы передаем экземпляр класса Receiver,
                                                                                           //то в коплексной команде могут быть использованы методы ресивера

            invoker.DoSomethingImportant(); //метод, который вызовет работу всего остального
        }
    }
}
