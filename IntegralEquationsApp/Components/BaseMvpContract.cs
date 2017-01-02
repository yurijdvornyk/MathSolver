namespace IntegralEquationsApp.Components
{
    public interface IView { }

    public interface IPresenter<V> where V : IView { }
}
