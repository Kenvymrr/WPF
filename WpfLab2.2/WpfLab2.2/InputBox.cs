using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

public class InputBox : Window
{
    private TextBox inputTextBox;
    private Button okButton;
    private int baseValue; // Store the base for validation

    public string InputValue { get; private set; }

    public InputBox(string prompt, int baseValue)
    {
        this.baseValue = baseValue; // Store the base
        Title = "Input";
        Width = 300;
        Height = 150;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        // Main Grid
        Grid grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        // Prompt Label
        Label promptLabel = new Label { Content = prompt, Margin = new Thickness(5) };
        Grid.SetRow(promptLabel, 0);
        grid.Children.Add(promptLabel);

        // Input TextBox
        inputTextBox = new TextBox { Margin = new Thickness(5) };
        Grid.SetRow(inputTextBox, 1);
        grid.Children.Add(inputTextBox);

        // OK Button
        okButton = new Button { Content = "OK", Width = 80, Height = 25, Margin = new Thickness(5) };
        okButton.Click += OkButton_Click;

        // StackPanel for Button
        StackPanel buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
        buttonPanel.Children.Add(okButton);
        Grid.SetRow(buttonPanel, 2);
        grid.Children.Add(buttonPanel);


        Content = grid;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Attempt to convert, throws exception if invalid
            Convert.ToInt32(inputTextBox.Text, baseValue);
            InputValue = inputTextBox.Text;
            DialogResult = true; // Close the dialog with a "true" result
        }
        catch (Exception)
        {
            MessageBox.Show("Invalid input for the specified base.  Please try again.");
            DialogResult = false;  // Keep the dialog open and wait for a valid input
        }

    }
}