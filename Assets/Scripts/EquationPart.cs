public interface EquationPart
{
    public EquationPart Add(EquationPart other);
    public EquationPart Subtract(EquationPart other);
    public EquationPart Multiply(EquationPart other);
    public EquationPart Divide(EquationPart other);
}