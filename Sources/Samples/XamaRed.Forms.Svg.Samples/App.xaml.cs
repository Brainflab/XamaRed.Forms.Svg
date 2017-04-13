using Xamarin.Forms;

namespace XamaRed.Forms.Svg.Samples
{
    public partial class App : Application
    {
        public App()
        {
            SvgView.ResourceIdsPrefix = "XamaRed.Forms.Svg.Samples.Assets.";
            InitializeComponent();
            MainPage = new MainPage();
        }
    }
}
