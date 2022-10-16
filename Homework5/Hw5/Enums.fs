namespace Hw5

type Message =
    | SuccessfulExecution = 0
    | WrongArgLength = 1
    | WrongArgFormat = 2
    | WrongArgFormatOperation = 3
    | DivideByZero = 4

type CalculatorOperation =
     | Plus = 0
     | Minus = 1
     | Multiply = 2
     | Divide = 3