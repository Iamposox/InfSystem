namespace IS.UI.Converters
{
    public static class ConvertersHost
    {

        private static PasswordToMaskedPasswordConverter _passwordToMaskedPasswordConverter;

        public static PasswordToMaskedPasswordConverter PasswordToMaskedPasswordConverter =>
            _passwordToMaskedPasswordConverter ?? (_passwordToMaskedPasswordConverter = new PasswordToMaskedPasswordConverter());

    }
}
