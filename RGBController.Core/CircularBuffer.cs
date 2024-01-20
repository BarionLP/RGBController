namespace RGBController.Core;

public sealed class FixedSizeBuffer<T>(int length) : List<T>{
    public int Length { get; set; } = length;

    public new void Add(T item){
        Insert(0, item);

        if (Count > Length){
            RemoveAt(Count - 1);
        }
    }
}
