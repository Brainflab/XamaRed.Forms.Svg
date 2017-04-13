using Xamarin.Forms;

namespace XamaRed.Forms.Svg.Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            StretchPicker.SelectedIndex = 0;
            HorizontalAlignmentPicker.SelectedIndex = 0;
            VerticalAlignmentPicker.SelectedIndex = 0;
            FilePicker.SelectedIndex = 0;
        }
    }
}
