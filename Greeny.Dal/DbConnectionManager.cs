using LinqToDB.Data;
using System.Collections.Generic;
using System.Threading;

namespace Greeny.Dal
{
    // Позволяет переиспользовать один DataConnection при обращении к БД из вложенных методов.
    // При использовании этого класса исключается ошибка 55000: prepared transactions are disabled
    // Ошибка возникает из за того, что при использовании в пределах TransactionScope нескольких DbConnection
    // поднимается распределенная транзакция, даже есть все DbConnection используют одинаковую строку подключения.
    internal class DbConnectionManager
    {
        private static readonly AsyncLocal<DbConnectionManager> localStack = new AsyncLocal<DbConnectionManager>();

        private static object syncRoot = new object();

        public static DbConnectionManager Instance
        {
            get
            {
                var stack = localStack.Value;
                if (stack == null)
                {
                    lock (syncRoot)
                    {
                        stack = localStack.Value;

                        if (stack == null)
                        {
                            stack = localStack.Value = new DbConnectionManager();
                        }
                    }
                }

                return stack;
            }
        }

        private Stack<DataConnection> stack = new Stack<DataConnection>();

        public bool IsEmpty => stack.Count == 0;

        public void Push(DataConnection connection)
        {
            stack.Push(connection);
        }

        public DataConnection Pop()
        {
            return stack.Pop();
        }

        public DataConnection Peek()
        {
            return stack.Peek();
        }

        public DataConnection PeekOrNull()
        {
            return stack.Count == 0 ? null : stack.Peek();
        }
    }
}
