using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern
{
    delegate void Invoker();
    class Command
    {
        public static Invoker Execute, Undo, Redo;
        public Command(Receiver receiver)
        {
            Execute = receiver.Action;
            Redo = receiver.Action;
            Undo = receiver.Reverse;
        }
    }
    public class Receiver
    {
        string build, oldbuild;
        string s = "some string ";
        public void Action()
        {
            oldbuild = build;
            build += s;
            Console.WriteLine("Receiver is adding " + build);
        }
        public void Reverse()
        {
            build = oldbuild;
            Console.WriteLine("Receiver is reverting to " + build);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Command c = new Command(new Receiver());
            Command.Execute();
            Command.Redo();
            Command.Undo();
            Command.Execute();
            Console.ReadKey();
        }
    }
}
