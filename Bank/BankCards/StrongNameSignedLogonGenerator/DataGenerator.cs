namespace StrongNameSignedLogonGenerator
{
    public class NewUser
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class DataGenerator
    {
        private Random random = new Random();
        const string StrUpper = "QWERTYUIOPASDFGHJKLZXCVBNM";
        const string StrLower = "qwertyuiopasdfghjklzxcvbnm";
        const string StrDigits = "0123456789";
        const string StrSpecial1 = "_-";
        const string StrSpecial2 = "!@#$%^";

        public NewUser GenerateSrting()
        {          
            int lenght = random.Next(8, 25);
            string login = GenerateString(lenght, true);

            lenght = random.Next(8, 25);
            string pass = GenerateString(lenght, false);

            return new NewUser{ Login = login, Password = pass };
        }

        private string GenerateString(int lenght, bool isLogin)
        {
            //соблюдаем правила = должно быть минимум
            // одна заглавная
            // одна строяная
            // одна цифра
            // один спецсимвол. Для логина усеченный список спецсимволов.

            char [] charArray = new char[lenght];
            int charArrayLenght = charArray.Length - 1;

            //Заглавная 
            SetCharToArray(charArray, StrUpper);

            //строчная 
            SetCharToArray(charArray, StrLower);

            //спецсимвол 
            string symbols = StrSpecial1;
            if (!isLogin)//если пароль 
            {
                symbols = symbols + StrSpecial2;
            }
            SetCharToArray(charArray, symbols);

            //цифра
            SetCharToArray(charArray, StrDigits);

            //заполняем оставшиеся позиции массива char
            symbols = StrUpper + StrLower + StrDigits + StrSpecial1;
            if (!isLogin)//если пароль 
            {
                symbols = symbols + StrSpecial2;
            }

            for (int i = 0; i <= charArrayLenght; i++)
            {
                if (charArray[i] == Char.MinValue)
                {
                    charArray[i] = GetRandomCharFromString(symbols);
                }
            }

            string result  = new string(charArray);
            return result;
        }

        private void SetCharToArray(char[] charArray, string stringSet)
        {
            bool isSettted = false;
            while (isSettted == false)
            {
                int charIndex = random.Next(0, charArray.Length - 1);

                if (charArray[charIndex] == Char.MinValue)
                {
                    charArray[charIndex] = GetRandomCharFromString(stringSet);
                    isSettted = true;
                }
            }
        }

        private char GetRandomCharFromString(string str)
        {
            return str[random.Next(0, str.Length - 1)];
        }
    }
}