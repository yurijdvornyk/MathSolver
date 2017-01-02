namespace IntegralEquationsApp.Components
{
    public class MvpPresenter<V> : IPresenter<V> where V : IView
    {
        protected V view;
        public MvpPresenter(V view)
        {
            this.view = view;
        }
    }
}
