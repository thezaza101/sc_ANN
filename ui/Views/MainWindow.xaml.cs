using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace ui.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var annTab = this.Get<TabControl>("TestName");
            
            var tbRaw = this.Get<TextBlock>("tbRawOutput");
            tbRaw.PropertyChanged += (s,e) => {
                var svRaw = this.Get<ScrollViewer>("svRawOutput");
                svRaw.Offset = new Vector(svRaw.Offset.X, svRaw.Extent.Height -svRaw.Viewport.Height);
            };

            Button btnCmd  = this.Get<Button>("btnRunCommand");
            var commandTextBox = this.Get<TextBox>("tbCurrentCommand");
            commandTextBox.KeyUp += (s,e) => {
                if(e.Key == Key.Enter)
                {
                    btnCmd.Command.Execute(s);        
                    e.Handled = true;       
                    //This causes an error                         
                }
            };
        }
             
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}