namespace IntegralEquationsApp.Components.InputData.ItemView
{
    /// <summary>
    /// Interaction logic for StringItemView.xaml
    /// </summary>
    public partial class StringItemView
    {
        public StringItemView()
        {
            InitializeComponent();
        }

        public override object GetItemValue()
        {
            return textBox.Text;
        }

        public override void SetItemValue(object value)
        {
            textBox.Text = value.ToString();
        }
    }
}
