using System.Collections;

namespace RGBController.Core;

public struct RandomInfinteIterator<T>(ICollection<T> values, Random random) : IEnumerator<T>{
    public T Current { get; private set; } = values.ElementAt(0);
    private readonly ICollection<T> _values = values;
    private readonly Random _random = random;
    private int _lastIndex = 0;

    public RandomInfinteIterator(ICollection<T> values) : this(values, new()){}

    public bool MoveNext(){
        int index;
        do{
            index = _random.Next(_values.Count);
        }while(index == _lastIndex);
        _lastIndex = index;
        Current = _values.ElementAt(index);
        return true;
    }

    public void Reset() {}
    public void Dispose(){}
    
    readonly object IEnumerator.Current => Current!;
}
