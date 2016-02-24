namespace EnemyFortress.Utilities
{
    class MyQueue<T>
    {
        /// <summary>
        /// Helper class
        /// </summary>
        private class Node
        {
            public T Data { get; set; }
            public Node NextNode { get; set; }

            /// <summary>
            /// Node constructor
            /// </summary>
            /// <param name="data"> data to be added </param>
            public Node(T data)
            {
                Data = data;
            }
        }

        /// <summary>
        /// Keep copies of both ends
        /// </summary>
        private Node lastNode = null;
        private Node firstNode = null;

        public int Count { get; private set; }
        public bool IsEmpty { get { return Count == 0; } }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public MyQueue()
        {

        }

        public MyQueue(MyQueue<T> newQueue)
        {
            this.Clear();
            EnqueueRange(newQueue);
        }

        /// <summary>
        /// Insert new data into the end of the queue
        /// </summary>
        /// <param name="data">Data to be added</param>
        public void Enqueue(T data)
        {
            Node newNode = new Node(data);

            if (IsEmpty)
                firstNode = lastNode = newNode;

            if (Count == 1)
                firstNode.NextNode = lastNode = newNode;

            lastNode.NextNode = newNode;
            lastNode = lastNode.NextNode;

            ++Count;
        }

        public void EnqueueRange(MyQueue<T> data)
        {
            while (!data.IsEmpty)
                this.Enqueue(data.Dequeue());
        }

        /// <summary>
        /// Remove and return first element in the queue
        /// </summary>
        public T Dequeue()
        {
            if (IsEmpty)
                return default(T);

            T result = firstNode.Data;

            firstNode = (Count > 1) ? firstNode.NextNode : null;

            --Count;

            return result;
        }

        /// <summary>
        /// Returns a copy of the first element in the queue
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return (IsEmpty) ? default(T) : firstNode.Data;
        }

        /// <summary>
        /// Delete all elements in the queue
        /// </summary>
        public void Clear()
        {
            while (!IsEmpty)
            {
                Dequeue();
            }
        }
    }
}
