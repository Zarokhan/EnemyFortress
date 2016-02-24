namespace EnemyFortress.Utilities
{
    class MyStack<T>
    {
        /// <summary>
        /// Helper class
        /// Contains element and a pointer to previously added node in stack
        /// </summary>
        private class Node
        {
            public T Data { get; set; }
            public Node PrevNode { get; set; }

            /// <summary>
            /// Node constructor
            /// </summary>
            /// <param name="data"> element to be addded to stack </param>
            /// <param name="prevNode"> previously added element in stack </param>
            public Node(T data, Node prevNode)
            {
                Data = data;
                PrevNode = prevNode;
            }
        }

        /// <summary>
        /// Properties and variables
        /// Head = top node in stack
        /// Count = number of elements in stack
        /// </summary>
        private Node head = null;
        public int Count { get; private set; }
        public bool IsEmpty { get { return Count == 0; } }

        /// <summary>
        /// Stack constructor
        /// </summary>
        public MyStack()
        {
        }

        /// <summary>
        /// Adds data to the top of the stack
        /// </summary>
        /// <param name="data"> data to be added </param>
        public void Push(T data)
        {
            head = new Node(data, head);
            ++Count;
        }

        /// <summary>
        /// Removes and returns top element in stack
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if (IsEmpty) return default(T);

            Node top = head;

            head = (Count > 1) ? head.PrevNode : null;
            --Count;

            return top.Data;
        }

        /// <summary>
        /// Returns top element in stack without removing it
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return (IsEmpty) ? default(T) : head.Data;
        }

        /// <summary>
        /// Removes all elements in stack
        /// </summary>
        public void Clear()
        {
            while (!IsEmpty)
                Pop();
        }
    }
}
