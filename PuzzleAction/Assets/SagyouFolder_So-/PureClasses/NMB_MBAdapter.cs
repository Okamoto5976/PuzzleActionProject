using UnityEngine;

public class NMB_MBAdapter<T>
{
    public T DeepCopy()
    {
        return (T)MemberwiseClone();
    }
}
