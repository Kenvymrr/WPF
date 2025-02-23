using System.Windows;

namespace CompilerWPF
{
    public partial class CompilationResultDialog : Window
    {
        public CompilationResultDialog(string results)
        {
            InitializeComponent();
            ResultTextBox.Text = results;
        }
    }
}