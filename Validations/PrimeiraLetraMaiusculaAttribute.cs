using System.ComponentModel.DataAnnotations;

class PrimeiraLetraMaiusculaAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var primeiraLetra = value.ToString()[0].ToString();
        if (primeiraLetra != primeiraLetra.ToUpper())
        {
            return new ValidationResult("A primeira letra deve ser maiúscula");
        }

        return ValidationResult.Success;
    }
}