using System.Windows.Controls;

namespace IntegralEquationsApp.Components.InputData.ItemView
{
    /// <summary>
    /// Interaction logic for IntegerItemView.xaml
    /// </summary>
    public partial class NumericIntegerItemView : UserControl, IBaseItemView<int>
    {

        public NumericIntegerItemView()
        {
            InitializeComponent();
        }

        public int getValue()
        {
            int result = 0;
            if (!int.TryParse(textBox.Text, out result))
            {
                throw new ItemValueParseException(GetType());
            }
            return result;
        }

        public void setValue(int value)
        {
            textBox.Text = value.ToString();
        }
    }
}
