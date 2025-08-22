namespace Utils.Management.Pooling
{
    public class Clone<T>
    {
        public Clone(T component)
        {
            Component = component;
        }
        
        public void Use() => IsUsing = true;

        public void Release() => IsUsing = false;

        public T Component { get; }
        
        public bool IsUsing { get; private set; }
    }
}
