// Calculator operations script
double PerformOperation(double num1, double num2, string op)
{
    switch(op)
    {
        case "+": return num1 + num2;
        case "-": return num1 - num2;
        case "*": return num1 * num2; 
        case "/":
            if (num2 == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero");
            }
            return num1 / num2;
        default:
            throw new ArgumentException("Invalid operator");
    }
}

var result = PerformOperation(Num1, Num2, Op);
Logger.LogInformation("{Num1} {Op} {Num2} = {Result}", Num1, Op, Num2, result);

return result;
