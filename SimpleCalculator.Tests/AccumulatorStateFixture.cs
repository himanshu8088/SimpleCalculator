﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCalculator.Core;
using SimpleCalculator.Core.Commands;
using SimpleCalculator.Core.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCalculator.Tests
{
    [TestClass]
    public class AccumulatorStateFixture
    {
        [TestMethod]
        public void DigitCommandShouldNotChangeStateTest()
        {
            Calculator calc = CalculatorFactory.BuildNew();
            calc.Notify(new DigitCommand(1));
            Assert.IsTrue(calc.State is AccumulatorState);

            calc.Notify(new DigitCommand(2));
            Assert.IsTrue(calc.State is AccumulatorState);
        }

        [TestMethod]
        public void ZeroDigitWithZeroAccumulatorIsInvariantTest()
        {
            Calculator calc = CalculatorFactory.BuildNew();
            calc.Notify(new DigitCommand(0));
            Assert.IsTrue(calc.State is AccumulatorState);
            Assert.IsTrue(calc.CPU.Accumulator == "0");
            calc.Notify(new DigitCommand(0));
            Assert.IsTrue(calc.CPU.Accumulator == "0");
        }

        [TestMethod]
        public void NonZeroDigitWithZeroAccumulatorShouldReplaceValueTest()
        {
            Calculator calc = CalculatorFactory.BuildNew();
            calc.Notify(new DigitCommand(0));
            Assert.IsTrue(calc.State is AccumulatorState);
            Assert.IsTrue(calc.CPU.Accumulator == "0");
            calc.Notify(new DigitCommand(1));
            Assert.IsTrue(calc.CPU.Accumulator == "1");
        }

        [TestMethod]
        public void AnyDigitWithNonZeroAccumulatorShouldAppendDigitTest()
        {
            Calculator calc = CalculatorFactory.BuildNew();
            calc.Notify(new DigitCommand(1));
            Assert.IsTrue(calc.State is AccumulatorState);
            Assert.IsTrue(calc.CPU.Accumulator == "1");
            calc.Notify(new DigitCommand(2));
            Assert.IsTrue(calc.CPU.Accumulator == "12");
        }


        [TestMethod]
        public void PointCommandShouldNotChangeStateTest()
        {
            Calculator calc = CalculatorFactory.BuildNew();
            calc.Notify(new DigitCommand(1));
            Assert.IsTrue(calc.State is AccumulatorState);

            calc.Notify(PointCommand.Instance);
            Assert.IsTrue(calc.State is AccumulatorState);
        }

        [TestMethod]
        public void PointCommandWithIntergerValueShouldAppendDotTest()
        {
            Calculator calc = CalculatorFactory.BuildNew();
            calc.Notify(new DigitCommand(1));
            Assert.IsTrue(calc.State is AccumulatorState);
            Assert.IsTrue(calc.CPU.Accumulator == "1");
            calc.Notify(PointCommand.Instance);
            Assert.IsTrue(calc.CPU.Accumulator == "1.");
        }

        [TestMethod]
        public void PointCommandWithDecimalValueShouldBeIgnoredTest()
        {
            Calculator calc = CalculatorFactory.BuildNew();
            calc.Notify(PointCommand.Instance);
            Assert.IsTrue(calc.State is AccumulatorState);
            Assert.IsTrue(calc.CPU.Accumulator == "0.");
            calc.Notify(PointCommand.Instance);
            Assert.IsTrue(calc.CPU.Accumulator == "0.");
        }


        [TestMethod]
        public void OperatorCommandWithEmptyStackTest()
        {
            Calculator calc = CalculatorFactory.BuildNew();
            calc.Notify(new DigitCommand(1));
            Assert.IsTrue(calc.State is AccumulatorState);
            calc.Notify(new DigitCommand(2));
            calc.Notify(new DigitCommand(3));
            // As of now accumulator state is 123

            calc.Notify(new OperatorCommand("+"));
            Assert.IsTrue(calc.CPU.OperandStack.Peek() == 123m);
            Assert.IsTrue(calc.CPU.OperatorStack.Peek() == "+");
            Assert.IsTrue(string.IsNullOrWhiteSpace(calc.CPU.Accumulator) == true);
        }

        [TestMethod]
        public void OperatorCommandWithNonEmptyStackTest()
        {
            Calculator calc = CalculatorFactory.BuildNew();
            calc.Notify(new DigitCommand(1));
            Assert.IsTrue(calc.State is AccumulatorState);
            calc.Notify(new DigitCommand(0));
            calc.Notify(new DigitCommand(0));
            // As of now accumulator state is 123
            calc.Notify(new OperatorCommand("+"));
            calc.Notify(new DigitCommand(2));
            calc.Notify(new DigitCommand(5));
            calc.Notify(new OperatorCommand("-"));
            Assert.IsTrue(calc.CPU.OperandStack.Peek() == 125m);
            Assert.IsTrue(calc.CPU.OperatorStack.Peek() == "-");
            Assert.IsTrue(string.IsNullOrWhiteSpace(calc.CPU.Accumulator) == true);

        }


        [TestMethod]
        public void BinaryOperatorCommandShouldNotChangeStateTest()
        {
            Calculator calc = CalculatorFactory.BuildNew();
            calc.Notify(new DigitCommand(1));
            Assert.IsTrue(calc.State is AccumulatorState);

            calc.Notify(new OperatorCommand("+"));
            Assert.IsTrue(calc.State is AccumulatorState);
        }

    }
}
