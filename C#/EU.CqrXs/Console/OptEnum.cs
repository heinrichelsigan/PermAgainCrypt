using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Zip;
using System;
using System.Collections.Generic;
using System.Text;

namespace EU.CqrXs.Console
{

    /// <summary>
    /// OptEnum different option types
    /// </summary>
    public enum OptEnum
    {
        Usage = 0x0,
        InParam = 0x1,
        Key = 0x2,
        Hash = 0x3,
        Zip = 0x4,
        CipherAlgos = 0x5,
        Encode = 0x6,
        OutP = 0x7,
        DeCrypt = 0x8,
        // SymmCipher = 0x9,
        SecureCipher = 0x10,
        Mode = 0xa,
        Verbose = 0xe,
        Help = 0xf
    }

    public static class OptEnumExtensions
    {

        /// <summary>
        /// Gets an option by argument
        /// </summary>
        /// <param name="argument">cmd line argument</param>
        /// <param name="optEnum"><see cref="OptEnum">OptEnum cmd arg option enum</see></param>
        /// <returns>optArg</returns>
        public static string[] GetOption(this string argument)
        {
            OptEnum optEnum = OptEnum.Usage;
            if (string.IsNullOrEmpty(argument) || argument.Length < 2 || argument[0] != '-')
            {
                optEnum = OptEnum.Usage;
                if (argument[0] != '/')
                    return new string[2] { optEnum.ToString(), argument };
            }

            string arg = argument.TrimStart("-/".ToCharArray());
            
            // optArg = arg.GetSubStringByPattern("=", true, "", " ", true, StringComparison.CurrentCultureIgnoreCase);
            string[] optArgs = (arg.Contains("=")) ?
                    arg.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries) :
                    new string[] { optEnum.ToString(), arg };           

            switch (arg[0])
            {
                case 'I':
                case 'i':
                    optEnum = OptEnum.InParam;
                    optArgs[0] = optEnum.ToString();
                    return optArgs;
                case 'O':
                case 'o':
                    optEnum = OptEnum.OutP;
                    optArgs[0] = optEnum.ToString();
                    return optArgs;
                case 'Z':
                case 'z':
                    optEnum = OptEnum.Zip;
                    optArgs[0] = optEnum.ToString();
                    return optArgs;
                case 'E':
                case 'e':
                    optEnum = OptEnum.Encode;
                    optArgs[0] = optEnum.ToString();
                    return optArgs;
                case 'C':
                case 'c':
                    optEnum = OptEnum.CipherAlgos;
                    optArgs[0] = optEnum.ToString();
                    return optArgs;
                case 'k':
                case 'K':
                    optEnum = OptEnum.Key;
                    optArgs[0] = optEnum.ToString();
                    return optArgs;
                case 'm':
                case 'M':
                    optEnum = OptEnum.Mode;
                    optArgs[0] = optEnum.ToString();
                    return optArgs;
                case 'h':
                case 'H':
                    optEnum = OptEnum.Hash;
                    optArgs[0] = optEnum.ToString();
                    return optArgs;
                case 'D':
                    optEnum = OptEnum.DeCrypt;
                    optArgs[0] = optEnum.ToString();
                    return optArgs;
                case 's':
                case 'S':
                    optEnum = OptEnum.SecureCipher;
                    optArgs[0] = optEnum.ToString();
                    return optArgs;
                case 'v':
                case 'V':
                    optEnum = OptEnum.Verbose;
                    optArgs[0] = optEnum.ToString();
                    return optArgs;
                case 'g':
                case 'G':
                case '?':
                    optEnum = OptEnum.Help;
                    optArgs[0] = optEnum.ToString();
                    optArgs[1] = "";
                    break;
                default:
                    optEnum = OptEnum.Usage;
                    optArgs[0] = optEnum.ToString();
                    optArgs[1] = $"unrecognized option: {argument}.";
                    break;
            }

            return optArgs;
        }

    }
}
