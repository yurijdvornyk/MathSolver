using ProblemSdk.Classes.Choice;
using System.Collections.Generic;

namespace IntegralEquationsApp.Components.InputData.ItemView
{
    /// <summary>
    /// Interaction logic for SingleChoiceItemView.xaml
    /// </summary>
    public partial class SingleChoiceItemView
    {
        private ISingleChoice singleChoice;

        public SingleChoiceItemView(ISingleChoice singleChoice)
        {
            InitializeComponent();
            this.singleChoice = singleChoice;
            comboBox.ItemsSource = singleChoice.GetOptions();
            comboBox.DataContext = this;
            if (singleChoice.GetValue() != null)
            {
                SetItemValue(singleChoice.GetValue());
            }
        }

        public override object GetItemValue()
        {
            singleChoice.SetValue(comboBox.SelectedItem);
            return singleChoice;
        }

        public override void SetItemValue(object value)
        {
            if (comboBox.Items.Contains(value))
            {
                comboBox.SelectedItem = value;
            }
        }
    }
}
