using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace Лабораторная
{
    class Analyz
    {
        private List<string> identifier;
        private List<string> nums;
        private List<string> D;
        private List<string> lytir;
        private List<string> allText;
        private int num_line;
        private int num_char;
        private string[] keywords;
        private string[] double_symvols;
        private char[] one_symvol;
        private string line;
        private Stack<char> brackets;
        private Stack<char> squareBrackets;

        public struct Token
        {
            public char a;
            public int num;
            public Token(char a, int num)
            {
                this.a = a;
                this.num = num;
            }

            public static bool operator ==(Token t1, Token t2)
            {
                if(t1.a == t2.a && t1.num == t2.num)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public static bool operator !=(Token t1, Token t2)
            {
                if (t1.a != t2.a || t1.num != t2.num)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public Analyz(string path)
        {
            num_line = 0;
            num_char = 0;

            allText = File.ReadAllLines(path).ToList<string>(); //TODO проверить на пустоту файла
            keywords = new string[] {
                 "and","bool","break","case","char",
                 "default","do","else","false","for",
                 "if","int","main","not","or",
                 "true","void","while","xor"};

            double_symvols = new string[] {
                            "==", "!=", ">=",
                            "<=", "+=", "-=",
                            "/=", "*=" };

            one_symvol = new char[]  {
                                '!', '(', ')', '*',
                                '+', ',', '-', '.',
                                '/', ':', ';', '<',
                                '=', '>', '?', '[',
                                ']', '{', '}' };
            identifier = new List<string>();
            nums = new List<string>();
            D = new List<string>();//double simvols
            lytir = new List<string>();

            brackets = new Stack<char>();
            squareBrackets = new Stack<char>();
        }

        private void Error(string message)
        {
            Console.WriteLine(message);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        public void RecursiveDescent()
        {
            El();
            Console.WriteLine("Верно!");
        }

        public void El() //or - K 14     
        {
            try
            {
                Tl();
                Token tmp = Scan(true);
                while ((tmp.a == 'K') && (tmp.num == 14))
                {
                    tmp = Scan(false);
                    Console.WriteLine("or");
                    Tl();
                    tmp = Scan(true);
                }
                tmp = Scan(true);
                if (tmp.a == new char() && tmp.num == new int())
                {
                    return;
                }
                if ((num_char != allText[num_line].Length) && brackets.Count == 0)
                {
                    throw new Exception();
                }
            }
            catch(Exception e)
            {
                Error("Неверно!");
            }
        }

        public void Tl() //and - K 0
        {
            Fl();
            Token tmp = Scan(true);
            while ((tmp.a == 'K') && (tmp.num == 0))
            {
                tmp = Scan(false);
                Console.WriteLine("and");
                Fl();
                tmp = Scan(true);
            }
        }

        public void Fl() // true - K 15    false - K 8   not - K 13
        {
            try
            {
                Token tmp = Scan(true);
                if (tmp.a == 'I')
                {
                    Console.WriteLine("массив");
                    tmp = Scan(false);
                    tmp = Scan(true);
                    if (tmp.a == 'R' && tmp.num == 15)
                    {
                        Fl();
                        tmp = Scan(true);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                if (tmp.a == 'K' && tmp.num == 10)
                {
                    Console.WriteLine("if");
                    tmp = Scan(false);
                    if (tmp.a == 'K' && (tmp.num == 0 || tmp.num == 14))
                    {
                        throw new Exception();
                    }
                    if (tmp.a == 'C' || tmp.a == 'I')
                    {
                        throw new Exception();
                    }
                    Fl();
                }
                if (tmp.a == 'K' && tmp.num == 13)
                {
                    Console.WriteLine("not");
                    if (tmp.a == 'K' && (tmp.num == 0 || tmp.num == 14))
                    {
                        throw new Exception();
                    }
                    if (tmp.a == 'C' || tmp.a == 'I')
                    {
                        throw new Exception();
                    }
                    tmp = Scan(false);
                    Fl();
                }
                if (tmp.a == 'R' && tmp.num == 15) // '[' - R 15, ']' - R 16
                {
                    if (tmp.a == 'K' && (tmp.num == 0 || tmp.num == 14))
                    {
                        throw new Exception();
                    }
                    tmp = Scan(false);
                    Console.WriteLine("Открылась квадратная скобка!");
                    squareBrackets.Push('[');
                    tmp = Scan(true);
                    if (tmp.a == 'K' && (tmp.num == 15 || tmp.num == 8)) //??
                    {
                        tmp = Scan(false);
                        if (tmp.num == 15)
                        {
                            Console.WriteLine("true");
                        }
                        else
                        {
                            Console.WriteLine("false");
                        }
                    }
                    else
                    {
                        E();
                    }
                    tmp = Scan(true);
                    if ((tmp.a == 'R' && tmp.num == 16) && (squareBrackets.Peek() == '['))
                    {
                        tmp = Scan(false);
                        squareBrackets.Pop();
                        Console.WriteLine("Квадратные скобки закрылись!");
                        return;
                    }
                    Zn();
                    tmp = Scan(false);
                    tmp = Scan(true);
                    if (tmp.a == 'K' && (tmp.num == 15 || tmp.num == 8)) //??
                    {
                        tmp = Scan(false);
                        if(tmp.num == 15)
                        {
                            Console.WriteLine("true");
                        }
                        else
                        {
                            Console.WriteLine("false");
                        }
                    }
                    else
                    {
                        E();
                    }
                    tmp = Scan(false);
                    if ((tmp.a == 'R' && tmp.num == 16) && (squareBrackets.Peek() == '['))
                    {
                        squareBrackets.Pop();
                        Console.WriteLine("Квадратные скобки закрылись!");
                        return;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                if (tmp.a == 'R' && tmp.num == 16)
                {
                    if (!squareBrackets.Any() || squareBrackets.Peek() == '(')
                    {
                        throw new Exception();
                    }
                }
                if (tmp.a == 'R' && tmp.num == 1)
                {
                    tmp = Scan(false);
                    Console.WriteLine("Открылась скобка!");
                    brackets.Push('(');
                    El();
                    tmp = Scan(false);
                    if ((tmp.a == 'R' && tmp.num == 2) && (brackets.Peek() == '('))
                    {
                        brackets.Pop();
                        Console.WriteLine("Скобка закрылась!");
                        return;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                if (tmp.a == 'R' && tmp.num == 2)
                {
                    if (!brackets.Any() || brackets.Peek() == '(')
                    {
                        throw new Exception();
                    }
                }
            }
            catch(Exception e)
            {
                Error("Неверно!");
            }
        }

        public void Zn()
        {
            try
            {
                Token tmp = Scan(true);

                Token[] arr =
                {
                    new Token('R', 13),   // > - R 13
                    new Token('R', 11),   // < - R 11
                    new Token('D', 0),    // == - D 0
                    new Token('D', 1),    // <> - != - D 1
                    new Token('D', 2),    // >= - D 2
                    new Token('D', 3)     // <= - D 3
                };

                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] == tmp)
                    {
                        Console.WriteLine("Обнаружен знак сравнения!");
                        return;
                    }
                }

                throw new Exception();
            }
            catch(Exception e)
            {
                Error("Неверно!");
            }
        }


        
        private void E()
        {
            try
            {
                T();
                Token tmp = Scan(true);
                while ((tmp.a == 'R') && ((tmp.num == 4) || (tmp.num == 6)))
                {
                    tmp = Scan(false);
                    if (tmp.num == 6)
                    {
                        Console.WriteLine("-");
                    }
                    if (tmp.num == 4)
                    {
                        Console.WriteLine("+");
                    }
                    T();
                    tmp = Scan(true);
                }
                tmp = Scan(true);
                if (tmp.a == new char() && tmp.num == new int())
                {
                    return;
                }
                if(((tmp.a == 'R')&&(tmp.num == 11 || tmp.num == 13 || tmp.num == 16)) || ((tmp.a == 'D') && (tmp.num == 0 || tmp.num == 1 || tmp.num == 2 || tmp.num == 3)))
                {
                    return;
                }
                if ((num_char != allText[num_line].Length) && brackets.Count == 0)
                {
                    throw new Exception();
                }
            }
            catch(Exception e)
            {
                Error("Неверно!");
            }

        }

        private void T()
        {
            F();
            Token tmp = Scan(true);
            while((tmp.a == 'R') && ((tmp.num == 3) || (tmp.num == 8)))
            {
                tmp = Scan(false);
                if (tmp.num == 3)
                {
                    Console.WriteLine("*");
                }
                if (tmp.num == 8)
                {
                    Console.WriteLine("/");
                }
                F();
                tmp = Scan(true);
            }
        }

        private void F()
        {
            try
            {
                Token tmp = Scan(true);
                if (tmp.a == 'I' || tmp.a == 'C')
                {
                    if(tmp.a == 'I')
                        Console.WriteLine("Идентификатор");
                    if (tmp.a == 'C')
                        Console.WriteLine("Число");
                    tmp = Scan(false);
                }
                if (tmp.a == 'R' && tmp.num == 1)
                {
                    tmp = Scan(false);
                    Console.WriteLine("Открылась скобка!");
                    brackets.Push('(');
                    E();
                    tmp = Scan(false);
                    if ((tmp.a == 'R' && tmp.num == 2) && (brackets.Peek() == '('))
                    {
                        brackets.Pop();
                        Console.WriteLine("Скобка закрылась!");
                        return;
                    }
                    else
                    {
                        throw new Exception("Ошибка!");
                    }
                }
                if (tmp.a == 'R' && tmp.num == 2)
                {
                    if (!brackets.Any() || brackets.Peek() == '(')
                    {
                        throw new Exception();
                    }
                }
            }
            catch(Exception e)
            {
                Error("Неверно!");
            }
            
        }

        public Token Scan(bool check)
        {
            bool isError = false;
            int old_condition, condition = 0;
            string buffer = "";
            int index_line = 0;
            line = allText[num_line];
            char[] arr = line.ToCharArray();
            for (int i = num_char; i < arr.Length; i++)
            {
                try
                {
                    if (isError)
                    {
                        break;
                    }
                    old_condition = condition;
                    if (condition != 4)
                    {
                        condition = CheckChar(arr[i]);
                    }

                    switch (condition)
                    {
                        case 0:
                            {

                            }
                            break;
                        case 1:
                            {
                                if (old_condition == condition)
                                {
                                    buffer += arr[i];
                                }
                                else
                                {
                                    buffer = "" + arr[i];
                                }
                                if ((i != arr.Length - 1) && (CheckChar(arr[i + 1]) != 1))
                                {
                                    if (CheckElement(keywords, buffer))
                                    {

                                        if (!check)
                                        {
                                            num_char = i + 1;
                                            num_line = index_line;
                                        }
                                        return new Token('K', GetIndexOfElement(keywords, buffer));
                                        //return "К[" + GetIndexOfElement(keywords, buffer) + "]   " + buffer;
                                    }
                                    else
                                    {
                                        if (!CheckElement(identifier, buffer))
                                        {
                                            identifier.Add(buffer);
                                        }
                                        if (!check)
                                        {
                                            num_char = i + 1;
                                            num_line = index_line;
                                        }
                                        return new Token('I', GetIndexOfElement(identifier, buffer));
                                    }
                                }
                                if (i == arr.Length - 1)// не учитывает знака
                                {
                                    if (CheckElement(keywords, buffer))
                                    {
                                        if (!check)
                                        {
                                            num_char = i + 1;
                                            num_line = index_line;
                                        }
                                        return new Token('K', GetIndexOfElement(keywords, buffer));
                                    }
                                    else
                                    {
                                        if (!CheckElement(identifier, buffer))
                                        {
                                            identifier.Add(buffer);
                                        }
                                        if (!check)
                                        {
                                            num_char = i + 1;
                                            num_line = index_line;
                                        }
                                        return new Token('I', GetIndexOfElement(identifier, buffer));
                                    }
                                }
                            }
                            break;
                        case 2: //C
                            {
                                if(i==arr.Length-1)
                                {
                                    if (!CheckElement(nums, buffer))
                                    {
                                        nums.Add(buffer);
                                    }
                                    if (!check)
                                    {
                                        num_char = i + 1;
                                        num_line = index_line;
                                    }
                                    return new Token('C', GetIndexOfElement(nums, buffer));
                                }
                                if (old_condition == condition)
                                {
                                    buffer += arr[i];
                                }
                                else
                                {
                                    buffer = "" + arr[i];
                                }
                                if (((i != arr.Length - 1) && (CheckChar(arr[i + 1]) != 2)) && (CheckChar(arr[i + 1]) != 10))
                                {
                                    if (!CheckElement(nums, buffer))
                                    {
                                        nums.Add(buffer);
                                    }
                                    if (!check)
                                    {
                                        num_char = i + 1;
                                        num_line = index_line;
                                    }
                                    return new Token('C', GetIndexOfElement(nums, buffer));
                                }
                            }
                            break;
                        case 3:
                            {
                                buffer = "";
                                if ((i != arr.Length - 1) && (arr[i + 1] == '='))
                                {
                                    buffer += arr[i];
                                    condition = 4;
                                }
                                else
                                {
                                    if ((i != arr.Length - 1) && (arr[i] == '/') && (CheckChar(arr[i + 1]) == 3))
                                    {
                                        buffer += arr[i];
                                        condition = 4;
                                    }
                                    else
                                    {
                                        buffer += arr[i];
                                        if (CheckElement(one_symvol, buffer))
                                        {
                                            if (!check)
                                            {
                                                num_char = i + 1;
                                                num_line = index_line;
                                            }
                                            return new Token('R', GetIndexOfElement(one_symvol, buffer));
                                        }
                                    }

                                }
                            }
                            break;
                        case 4:
                            {
                                bool isDoubleSymvol = false;
                                buffer += arr[i];

                                if (buffer == "//")
                                {
                                    do
                                    {
                                        i++;
                                        buffer += arr[i];
                                    }
                                    while (i != arr.Length - 1);
                                    Console.WriteLine("Comment: " + buffer);
                                    condition = 0;
                                }
                                else if ((buffer == "/*") || ((buffer[0] == '/') && (buffer[1] == '*')))
                                {
                                    do
                                    {
                                        i++;
                                        buffer += arr[i];
                                        if ((arr[i] == '*') && (arr[i + 1] == '/'))
                                        {
                                            i++;
                                            buffer += arr[i];
                                            condition = 0;
                                            Console.WriteLine("Comment: " + buffer);
                                            break;
                                        }
                                    }
                                    while (i != arr.Length - 1);

                                }
                                else
                                {
                                    for (int j = 0; j < double_symvols.Length; j++)
                                    {
                                        if (double_symvols[j] == buffer)
                                        {
                                            isDoubleSymvol = true;
                                            break;
                                        }
                                    }
                                    if (isDoubleSymvol)
                                    {
                                        D.Add(buffer);
                                        if (!check)
                                        {
                                            num_char = i + 1;
                                            num_line = index_line;
                                        }
                                        return new Token('D', GetIndexOfElement(double_symvols, buffer));
                                    }
                                    else
                                    {
                                        if (CheckElement(one_symvol, arr[i - 1]))
                                        {
                                            if (!check)
                                            {
                                                num_char = i;
                                                num_line = index_line;
                                            }
                                            return new Token('R', GetIndexOfElement(one_symvol, buffer));
                                        }
                                        if (CheckElement(one_symvol, arr[i]))
                                        {
                                            if (!check)
                                            {
                                                num_char = i + 1;
                                                num_line = index_line;
                                            }
                                            return new Token('R', GetIndexOfElement(one_symvol, buffer));
                                        }
                                    }
                                    condition = 0;
                                }
                            }
                            break;
                        case 5:
                            {
                                buffer = "";
                                do
                                {
                                    i++;
                                    if (CheckChar(arr[i]) == 5)
                                    {
                                        break;
                                    }
                                    buffer += arr[i];
                                }
                                while (CheckChar(arr[i]) != 5);

                                lytir.Add(buffer);
                                if (!check)
                                {
                                    num_char = i + 1;
                                    num_line = index_line;
                                }
                                return new Token('L', GetIndexOfElement(lytir, buffer));
                            }
                            break;
                        default:
                            {
                                throw new Exception();
                            }
                            break;
                    }
                    if (i == arr.Length - 1)
                    {
                        num_char++;
                    }
                }
                catch(Exception e)
                {
                    
                    Error("Был введен недопустимый символ "+ allText[index_line][i]+ "!");
                    break;
                }
            }
            return new Token(new char(), new int());
        }
        private int CheckChar(char x)
        {
            if (x == ' ')
                return 0;

            if ((x >= 'a' && x <= 'z') || (x >= 'A' && x <= 'Z') || (x == '_'))
            {
                return 1;
            }

            if ((x == 34) || (x == 39))
            {
                return 5;
            }

            if (x >= '0' && x <= '9')
            {
                return 2;
            }

            for (int i = 0; i < one_symvol.Length; i++)
            {
                if (x == one_symvol[i])
                {
                    return 3;
                }
            }

            return -1;
        }
        private bool CheckElement(char[] list, string element)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == element[0])
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckElement(char[] list, char element)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == element)
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckElement(List<string> list, string element)
        {
            foreach (string arg in list)
            {
                if (arg == element)
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckElement(string[] list, string element)
        {
            foreach (string arg in list)
            {
                if (arg == element)
                {
                    return true;
                }
            }
            return false;
        }
        private int GetIndexOfElement(string[] list, string element)
        {
            int index = -1;
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == element)
                {
                    index = i;
                }
            }
            return index;
        }
        private int GetIndexOfElement(char[] list, string element)
        {
            int index = -1;
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == element[0])
                {
                    index = i;
                }
            }
            return index;
        }
        private int GetIndexOfElement(char[] list, char element)
        {
            int index = -1;
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == element)
                {
                    index = i;
                }
            }
            return index;
        }
        private int GetIndexOfElement(List<string> list, string element)
        {
            int index = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == element)
                {
                    index = i;
                }
            }
            return index;
        }
    }
}
