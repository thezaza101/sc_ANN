using Avalonia;
using Avalonia.Markup.Xaml;

namespace sc_ANN
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
   }
}