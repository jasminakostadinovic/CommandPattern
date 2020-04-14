using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern_RealanPrimer
{
    delegate void Invoker();
    static class InvokerExtensions
    {
        static int count;
        public static void Log(this Invoker invoker)
        {
            count++;
        }
        public static int Count(this Invoker invoker)
        {
            return count;
        }

    }

    abstract class ICommand
    {
        public Invoker Execute, Redo, Undo;
    }
    // Command 1
    class Paste : ICommand
    {
        public Paste(Document document)
        {
            Execute = delegate { Execute.Log(); document.Paste(); };
            Redo = delegate { Redo.Log(); document.Paste(); };
            Undo = delegate { Undo.Log(); document.Restore(); };
        }
    }
    // Command 2 - without an Undo method
    class Print : ICommand
    {
        public Print(Document document)
        {
            Execute = delegate { Redo.Log(); document.Print(); };
            Redo = delegate { Redo.Log(); document.Print(); };
            Undo = delegate {
                Redo.Log(); Console.WriteLine(
"Cannot undo a Print ");
            };
        }
    }
    public class ClipboardSingleton
    {
        public string Clipboard { get; set; }
        // Private Constructor
        ClipboardSingleton() { }
        // Private object instantiated with private constructor
        static readonly ClipboardSingleton instance = new ClipboardSingleton();
        // Public static property to get the object
        public static ClipboardSingleton UniqueInstance
        {
            get { return instance; }
        }
    }

    // Receiver
    class Document
    {
        string name;
        string oldpage, page;
        public Document(string name)
        {
            this.name = name;
        }
        public void Paste()
        {
            oldpage = page;
            page += ClipboardSingleton.UniqueInstance.Clipboard + "\n";
        }
        public void Restore()
        {
            page = oldpage;
        }
        public void Print()
        {
            Console.WriteLine(
            "File " + name + " at " + DateTime.Now + "\n" + page);
        }
    }

    class Program
    {
        
        static void Main(string[] args)
        {
            Document document = new Document("Greetings");
            Paste paste = new Paste(document);
            Print print = new Print(document);
            ClipboardSingleton.UniqueInstance.Clipboard = "Hello, everyone";
            paste.Execute();
            print.Execute();
            paste.Undo();
            ClipboardSingleton.UniqueInstance.Clipboard = "Bonjour, mes amis";
            paste.Execute();
            ClipboardSingleton.UniqueInstance.Clipboard = "Guten morgen";
            paste.Redo();
            print.Execute();
            print.Undo();
            Console.WriteLine("Logged " + paste.Execute.Count() + " commands");
            Console.ReadKey();

        }
    }
}
