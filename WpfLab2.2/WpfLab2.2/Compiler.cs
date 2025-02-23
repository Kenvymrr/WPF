using System;
using System.Collections.Generic;

namespace CompilerWPF
{
    public class Compiler
    {
        public List<int> Compile(string code)
        {
            List<int> instructions = new List<int>();
            string[] lines = code.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string[] parts = line.Trim().Split(' ');
                if (parts.Length == 0) continue; // Skip empty lines

                try
                {
                    int opcode = ParseOpcode(parts[0]);
                    int operand1 = (parts.Length > 1) ? ParseOperand(parts[1]) : 0;
                    int operand2 = (parts.Length > 2) ? ParseOperand(parts[2]) : 0;
                    int operand3 = (parts.Length > 3) ? ParseOperand(parts[3]) : 0;

                    int instruction = opcode | (operand1 << 5) | (operand2 << 14) | (operand3 << 23);
                    instructions.Add(instruction);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error compiling line: '{line}'.  Reason: {ex.Message}");
                }
            }

            return instructions;
        }

        private int ParseOpcode(string opcodeString)
        {
            if (int.TryParse(opcodeString, out int opcode) && opcode >= 0 && opcode <= 24)
            {
                return opcode;
            }
            throw new ArgumentException("Invalid opcode: " + opcodeString);
        }

        private int ParseOperand(string operandString)
        {
            if (int.TryParse(operandString, out int operand) && operand >= 0 && operand < 512)
            {
                return operand;
            }
            throw new ArgumentException("Invalid operand: " + operandString);
        }
    }
}