namespace IntegralEquationsApp.Components
{
    public class Presenter<V> : IPresenter<V> where V : IView
    {
        protected V view;
        public Presenter(V view)
        {
            this.view = view;
        }
    }
}
