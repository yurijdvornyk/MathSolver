namespace IntegralEquationsApp.Components.InputData.ItemView
{
    public interface IBaseItemView<T>
    {
        void setValue(T value);
        T getValue();
    }
}
