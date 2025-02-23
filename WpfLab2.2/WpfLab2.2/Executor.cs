using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CompilerWPF
{
    public class Executor
    {
        private int[] registers;
        private MainWindow mainWindow; // Ссылка на главное окно
        private Dictionary<int, int> changedRegisters = new Dictionary<int, int>();
        private Dictionary<int, int> registerCallCounts = new Dictionary<int, int>();

        public Executor(int[] registers, MainWindow mainWindow)
        {
            this.registers = registers;
            this.mainWindow = mainWindow;
        }

        public void Execute(List<int> instructions)
        {
            changedRegisters.Clear();
            registerCallCounts.Clear(); // Reset call counts for each execution

            foreach (int instruction in instructions)
            {
                int opcode = instruction & 0x1F; // Extract lower 5 bits (opcode)
                int operand1 = (instruction >> 5) & 0x1FF; // Extract next 9 bits (operand 1)
                int operand2 = (instruction >> 14) & 0x1FF; // Extract next 9 bits (operand 2)
                int operand3 = (instruction >> 23) & 0x1FF; // Extract next 9 bits (operand 3)

                switch (opcode)
                {
                    case 0: // Print registers in specified base
                        LogRegisterChanges(operand1);
                        break;
                    case 1: // Bitwise NOT
                        registers[operand3] = ~registers[operand1];
                        TrackRegisterChange(operand3);
                        break;
                    case 2: // OR
                        registers[operand3] = registers[operand1] | registers[operand2];
                        TrackRegisterChange(operand3);
                        break;
                    case 3: // AND
                        registers[operand3] = registers[operand1] & registers[operand2];
                        TrackRegisterChange(operand3);
                        break;
                    case 4: // XOR
                        registers[operand3] = registers[operand1] ^ registers[operand2];
                        TrackRegisterChange(operand3);
                        break;
                    case 5: // Implication
                        registers[operand3] = (~registers[operand1] | registers[operand2]);
                        TrackRegisterChange(operand3);
                        break;
                    case 6: // Coimplication
                        registers[operand3] = (registers[operand1] | ~registers[operand2]);
                        TrackRegisterChange(operand3);
                        break;
                    case 7: // Equivalence
                        registers[operand3] = ~(registers[operand1] ^ registers[operand2]);
                        TrackRegisterChange(operand3);
                        break;
                    case 8: // Pierce's Arrow (NOR)
                        registers[operand3] = ~(registers[operand1] | registers[operand2]);
                        TrackRegisterChange(operand3);
                        break;
                    case 9: // Sheffer Stroke (NAND)
                        registers[operand3] = ~(registers[operand1] & registers[operand2]);
                        TrackRegisterChange(operand3);
                        break;
                    case 10: // Addition
                        registers[operand3] = registers[operand1] + registers[operand2];
                        TrackRegisterChange(operand3);
                        break;
                    case 11: // Subtraction
                        registers[operand3] = registers[operand1] - registers[operand2];
                        TrackRegisterChange(operand3);
                        break;
                    case 12: // Multiplication
                        registers[operand3] = registers[operand1] * registers[operand2];
                        TrackRegisterChange(operand3);
                        break;
                    case 13: // Integer Division
                        if (registers[operand2] == 0) throw new DivideByZeroException("Division by zero.");
                        registers[operand3] = registers[operand1] / registers[operand2];
                        TrackRegisterChange(operand3);
                        break;
                    case 14: // Modulo
                        if (registers[operand2] == 0) throw new DivideByZeroException("Division by zero.");
                        registers[operand3] = registers[operand1] % registers[operand2];
                        TrackRegisterChange(operand3);
                        break;
                    case 15: // Swap
                        int temp = registers[operand1];
                        registers[operand1] = registers[operand2];
                        registers[operand2] = temp;
                        TrackRegisterChange(operand1);
                        TrackRegisterChange(operand2);
                        break;
                    case 16: // Set Byte
                        int byteIndex = registers[operand2];
                        if (byteIndex < 0 || byteIndex >= 4) throw new IndexOutOfRangeException("Byte index out of range (0-3).");
                        int byteValue = registers[operand3] & 0xFF; // Ensure only the lowest byte is used

                        byte[] bytes = BitConverter.GetBytes(registers[operand1]);

                        bytes[byteIndex] = (byte)byteValue;

                        registers[operand1] = BitConverter.ToInt32(bytes, 0);
                        TrackRegisterChange(operand1);
                        break;
                    case 17: // Print Operand in Base
                        PrintOperandInBase(operand1, operand2);
                        break;
                    case 18: // Input
                        InputOperand(operand1, operand2);
                        break;
                    case 19: // Largest Power of 2
                        registers[operand3] = LargestPowerOfTwo(registers[operand1]);
                        TrackRegisterChange(operand3);
                        break;
                    case 20: // Left Shift
                        registers[operand3] = registers[operand1] << registers[operand2];
                        TrackRegisterChange(operand3);
                        break;
                    case 21: // Right Shift
                        registers[operand3] = registers[operand1] >> registers[operand2];
                        TrackRegisterChange(operand3);
                        break;
                    case 22: // Rotate Left
                        registers[operand3] = RotateLeft(registers[operand1], registers[operand2]);
                        TrackRegisterChange(operand3);
                        break;
                    case 23: // Rotate Right
                        registers[operand3] = RotateRight(registers[operand1], registers[operand2]);
                        TrackRegisterChange(operand3);
                        break;
                    case 24: // Copy
                        registers[operand1] = registers[operand2];
                        TrackRegisterChange(operand1);
                        break;
                    default:
                        throw new InvalidOperationException("Invalid opcode: " + opcode);
                }
            }
            LogAllChangedRegisters(10);
            mainWindow.UpdateRegisterDisplay(registerCallCounts);
        }

        private void TrackRegisterChange(int registerIndex)
        {
            if (!changedRegisters.ContainsKey(registerIndex))
            {
                changedRegisters[registerIndex] = registers[registerIndex];
            }
            if (registerCallCounts.ContainsKey(registerIndex))
            {
                registerCallCounts[registerIndex]++;
            }
            else
            {
                registerCallCounts[registerIndex] = 1;
            }
        }

        private void LogRegisterChanges(int baseValue)
        {
            mainWindow.AppendToOutput("PrintRegisters was called\n");
            if (baseValue < 2 || baseValue > 36)
            {
                mainWindow.AppendToOutput("Invalid base.  Using base 10.\n");
                baseValue = 10;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var register in changedRegisters)
            {
                sb.AppendLine($"Register {register.Key}: {Convert.ToString(registers[register.Key], baseValue)}");
            }
            mainWindow.AppendToOutput(sb.ToString());
        }

        private void LogAllChangedRegisters(int baseValue)
        {
            if (baseValue < 2 || baseValue > 36)
            {
                mainWindow.AppendToOutput("Invalid base.  Using base 10.\n");
                baseValue = 10;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var register in changedRegisters)
            {
                sb.AppendLine($"Register {register.Key}: {Convert.ToString(registers[register.Key], baseValue)}");
            }
            mainWindow.AppendToOutput(sb.ToString());
        }

        private void PrintOperandInBase(int operand, int baseValue)
        {
            mainWindow.AppendToOutput("PrintOperandInBase was called\n");
            if (baseValue < 2 || baseValue > 36)
            {
                mainWindow.AppendToOutput("Invalid base.  Using base 10.\n");
                baseValue = 10;
            }

            mainWindow.AppendToOutput($"Register {operand} in base {baseValue}: {Convert.ToString(registers[operand], baseValue)}\n");
        }

        private void InputOperand(int operand, int baseValue)
        {
            InputBox inputBox = new InputBox($"Enter value for register {operand} in base {baseValue}:", baseValue);
            if (inputBox.ShowDialog() == true)
            {
                try
                {
                    registers[operand] = Convert.ToInt32(inputBox.InputValue, baseValue);
                }
                catch (Exception)
                {
                    MessageBox.Show("Invalid input.  Please enter a valid number in the specified base.");
                }
            }
        }

        private int LargestPowerOfTwo(int number)
        {
            if (number <= 0) return 0;

            int power = 1;
            while ((power * 2) <= number)
            {
                power *= 2;
            }
            return power;
        }

        private int RotateLeft(int value, int bits)
        {
            bits %= 32; // Ensure bits is within 0-31
            return (value << bits) | (value >> (32 - bits));
        }

        private int RotateRight(int value, int bits)
        {
            bits %= 32; // Ensure bits is within 0-31
            return (value >> bits) | (value << (32 - bits));
        }
    }
}