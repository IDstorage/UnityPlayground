namespace UP08
{
    public class CustomLinkedList<T> where T : class
    {
        public class Iterator
        {
            public CustomLinkedList<T> Current;
            public static Iterator operator ++(Iterator target)
            {
                if (target.Current == null) return null;
                if (target.Current.IsEmpty) return null;
                if (target.Current.Next == null) return null;

                target.Current = target.Current.Next;
                return target;
            }
        }

        public T Data { get; set; }
        public CustomLinkedList<T> Next { get; set; } = null;

        public bool IsEmpty => Data == null && Next == null;

        public Iterator Begin => IsEmpty ? null : new Iterator() { Current = this };

        public void Add(T data)
        {
            Add(new CustomLinkedList<T>()
            {
                Data = data
            });
        }
        public void Add(CustomLinkedList<T> node)
        {
            if (IsEmpty)
            {
                this.Data = node.Data;
                this.Next = node.Next;
                return;
            }

            if (Next == null)
            {
                Next = node;
                return;
            }

            node.AddLast(this.Next);
            this.Next = node;
        }

        public void AddLast(T data)
        {
            AddLast(new CustomLinkedList<T>()
            {
                Data = data
            });
        }
        public void AddLast(CustomLinkedList<T> node)
        {
            if (IsEmpty)
            {
                this.Data = node.Data;
                this.Next = node.Next;
                return;
            }

            var search = this;
            while (search.Next != null)
            {
                search = search.Next;
            }

            search.Next = node;
        }

        public void Remove(T data)
        {
            if (ReferenceEquals(this.Data, data))
            {
                if (Next == null)
                {
                    Data = null;
                    return;
                }
                this.Data = this.Next.Data;
                this.Next = this.Next.Next;
                return;
            }

            var parent = this;
            var search = this.Next;
            while (search != null)
            {
                if (!ReferenceEquals(search.Data, data))
                {
                    parent = search;
                    search = search.Next;
                    continue;
                }

                parent.Next = search.Next;

                search.Data = null;
                search.Next = null;
                return;
            }
        }

        public T Find(System.Func<T, bool> compare)
        {
            for (var iter = Begin; iter != null; ++iter)
            {
                if (iter.Current.Data == null) continue;

                if (compare.Invoke(iter.Current.Data)) return iter.Current.Data;
            }
            return null;
        }

        public void Clear()
        {
            Data = null;
            Next = null;
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();

            var search = this;
            while (search != null)
            {
                if (search.Data == null) break;
                sb.Append($"[ {search.Data.ToString()} ] -> ");

                search = search.Next;
            }
            sb.Append("NULL");

            return sb.ToString();
        }
    }
}