using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32; // For OpenFileDialog

namespace CompilerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[] registers = new int[512]; // 512 registers as each index is 9 bits
        private Compiler compiler = new Compiler(); // Instance of the Compiler class
        private Executor executor; //Instance of the Executor class

        public MainWindow()
        {
            InitializeComponent();
            executor = new Executor(registers, this); // Pass MainWindow instance to Executor

            // No example code is added initially
            // CodeTextBox.Text = "10 1 2 3\n11 4 5 6\n0 10"; // Remove this line
            UpdateRegisterDisplay(new Dictionary<int, int>()); // Initial call with empty dictionary
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // Compile and execute from the editor
            string code = CodeTextBox.Text;
            CompileAndExecute(code);
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|Binary Files (*.bin)|*.bin|All Files (*.*)|*.*"; // Modified filter

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = openFileDialog.FileName;

                    // Check if the selected file is a text file
                    if (filePath.ToLower().EndsWith(".txt"))
                    {
                        // Load the text from the file into the CodeTextBox
                        CodeTextBox.Text = File.ReadAllText(filePath);
                    }
                    else if (filePath.ToLower().EndsWith(".bin")) // If it's a binary file
                    {
                        byte[] fileBytes = File.ReadAllBytes(filePath);
                        using (MemoryStream ms = new MemoryStream(fileBytes))
                        {
                            CompileAndExecuteFromStream(ms);
                        }
                    }

                }
                catch (Exception ex)
                {
                    AppendToOutput("Error reading or processing file: " + ex.Message);
                }
            }
        }

        private void CompileAndExecute(string code)
        {
            string compilationResults = "";
            List<int> compiledInstructions = null;
            try
            {
                compiledInstructions = compiler.Compile(code); // Use the Compiler class
                compilationResults = "Compilation successful!\n";
                foreach (int instruction in compiledInstructions)
                {
                    compilationResults += instruction.ToString("X8") + "\n"; // Display in hex for easier readability
                }

            }
            catch (Exception ex)
            {
                compilationResults = "Compilation error: " + ex.Message;
            }
            finally
            {
                CompilationResultDialog resultDialog = new CompilationResultDialog(compilationResults);
                resultDialog.ShowDialog();

                if (compiledInstructions != null)
                {
                    try
                    {
                        executor.Execute(compiledInstructions);
                    }
                    catch (Exception ex)
                    {
                        AppendToOutput("Execution error: " + ex.Message);
                    }
                }
            }
        }

        private void CompileAndExecuteFromStream(MemoryStream stream)
        {
            try
            {
                List<int> instructions = ReadInstructionsFromStream(stream);
                AppendToOutput("Instructions read from stream.\n");
                foreach (int instruction in instructions)
                {
                    AppendToOutput(instruction.ToString("X8") + "\n"); // Display in hex
                }

                executor.Execute(instructions);
            }
            catch (Exception ex)
            {
                AppendToOutput("Error processing stream: " + ex.Message);
            }
        }

        private List<int> ReadInstructionsFromStream(MemoryStream stream)
        {
            List<int> instructions = new List<int>();
            using (BinaryReader reader = new BinaryReader(stream))
            {
                while (stream.Position < stream.Length)
                {
                    try
                    {
                        instructions.Add(reader.ReadInt32()); // Read each 4 bytes as an integer
                    }
                    catch (EndOfStreamException)
                    {
                        // Handle unexpected end of stream (e.g., incomplete instruction)
                        AppendToOutput("Warning: Incomplete instruction at end of stream.\n");
                        break;  // Exit loop as we can't read a full instruction
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error reading instruction from stream: " + ex.Message);
                    }
                }
            }
            return instructions;
        }

        public void UpdateRegisterDisplay(Dictionary<int, int> registerCallCounts)
        {
            RegisterValuesTextBox.Clear();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 24; i++) // Display first 24 registers for brevity
            {
                int callCount = registerCallCounts.ContainsKey(i) ? registerCallCounts[i] : 0;
                sb.AppendLine($"R{i}: {registers[i]} (Calls: {callCount})");
            }
            RegisterValuesTextBox.Text = sb.ToString();
        }

        private void ClearRegistersButton_Click(object sender, RoutedEventArgs e)
        {
            Array.Clear(registers, 0, registers.Length);
            UpdateRegisterDisplay(new Dictionary<int, int>());
        }

        public void AppendToOutput(string text)
        {
            OutputTextBox.AppendText(text);
            OutputTextBox.ScrollToEnd(); // Автоматическая прокрутка к концу
        }
    }
}    